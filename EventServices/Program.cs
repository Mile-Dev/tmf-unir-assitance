using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda;
using Amazon.S3;
using Amazon.SQS;
using EventServices.Common.Validators;
using EventServices.Controllers;
using EventServices.Controllers.EventFirstContact;
using EventServices.EventFirstContact.Infraestructure.DataAccesDynamodb;
using EventServices.EventFirstContact.Infraestructure.DataAccesDynamodb.Interface;
using EventServices.EventFirstContact.Services;
using EventServices.EventFirstContact.Services.Factory;
using EventServices.EventFirstContact.Services.Factory.Interfaces;
using EventServices.EventFirstContact.Services.Interfaces;
using EventServices.EventFirstContact.Services.Strategy.Interfaces;
using EventServices.Infraestructura.AuditLog;
using EventServices.Infraestructura.DataAccess;
using EventServices.Infraestructura.DataAccess.Common;
using EventServices.Infraestructura.DataAccess.Interface;
using EventServices.Infraestructura.EndpointFirstContact;
using EventServices.Infraestructura.LambdaFirstContact;
using EventServices.Infraestructura.Security;
using EventServices.Services;
using EventServices.Services.Factory;
using EventServices.Services.Factory.Interfaces;
using EventServices.Services.Interfaces;
using EventServices.Services.Interfaces.Security;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SQSProducerServices.Common.Interfaces;
using StorageS3Services.Common.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Configure AWS options
// Asume que la configuraci�n de AWS se carga de appsettings.json o variables de entorno
// se instala esta libreria  => dotnet add package AWSSDK.Extensions.NETCore.Setup
var awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();
builder.Services.AddScoped(typeof(IEventFirstContactRepository<>), typeof(EventFirstContactRepository<>));
builder.Services.AddAWSService<IAmazonSQS>();
builder.Services.AddAWSService<IAmazonS3>();

//Add Databases
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MainContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mySqlOptions => mySqlOptions.EnableRetryOnFailure(
        maxRetryCount: 5,
        maxRetryDelay: TimeSpan.FromSeconds(10),
        errorNumbersToAdd: null
        ));
});

//Add Security
builder.Services.Configure<ClientKeyApiMappingOptions>(
    builder.Configuration.GetSection("ClientKeyApiMappings"));
builder.Services.AddSingleton<IApiKeyClientMapper, AppSettingsApiKeyClientMapper>();

//Add Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Add Services
builder.Services.AddScoped<IS3Service, S3Service>();
builder.Services.AddScoped<IInvokeIntegrationFirstContact, InvokeIntegrationFirstContact>();
builder.Services.AddSingleton<IAmazonLambda, AmazonLambdaClient>();
builder.Services.AddScoped<ILambdaFirstContact, LambdaFirstContact>();
builder.Services.AddScoped<IViewEventsServices, ViewEventsServices>();
builder.Services.AddScoped<IViewPhoneConsultationEventsServices, ViewPhoneConsultationEventsServices>();
builder.Services.AddScoped<IEventProviderServices, EventProviderServices>();
builder.Services.AddScoped<IGuaranteePaymentServices, GuaranteePaymentServices>();
builder.Services.AddScoped<IViewGuaranteesPaymentEventProviderServices, ViewGuaranteesPaymentEventProviderServices>();
builder.Services.AddScoped<IEventFirstContactServices, EventFirstContactServices>();
builder.Services.AddScoped<INotesSqsServices, NotesSqsServices>();
builder.Services.AddScoped<ISqsService, SqsService>();
builder.Services.AddScoped<IEventStatusChangeServices, EventStatusChangeServices>();
builder.Services.AddScoped<IPhoneConsultationServices, PhoneConsultationServices>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();
builder.Services.AddScoped<IDocumentServices, EventServices.Services.DocumentServices>();

//Add Handler And Factory
builder.Services.Scan(scan => scan
    .FromAssemblyOf<IEventFirstContactHandler>()
    .AddClasses(classes => classes.AssignableTo<IEventFirstContactHandler>())
    .As<IEventFirstContactHandler>()
    .WithScopedLifetime());

builder.Services.AddScoped<IEventFirstContactHandlerFactory, EventFirstContactHandlerFactory>();

builder.Services.Scan(scan => scan
    .FromAssemblyOf<IEventCreationStrategy>()
    .AddClasses(classes => classes.AssignableTo<IEventCreationStrategy>())
    .As<IEventCreationStrategy>()
    .WithScopedLifetime());
builder.Services.AddScoped<IEventCreationStrategyFactory, EventCreationStrategyFactory>();


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(
    options =>
    {
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "API Event",
            Version = "v1"
        });
    }
);

builder.Services.AddValidatorsFromAssemblyContaining<RequestEventValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Event v1");
            c.RoutePrefix = "swagger";
        }
        );
}
;

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
;


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda");

app.MapEventDraftCreateEndpoints();
app.MapViewPhoneConsultationEventGetDtoEndpoints();
app.MapEventProviderDtoEndpoints();
app.MapGuaranteePaymentDtoEndpoints();
app.MapResponseFirstContactEndpoints();

app.MapResquestLogDataEndpoints();

app.MapRequestEventStatusEndpoints();

app.MapPhoneConsultationEndpoints();

app.MapS3ServiceEndpoints();

app.Run();
