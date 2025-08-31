using System.Text.Json;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using ProviderService.Domain.Interfaces;
using ProviderService.Infraestructura.DataAccess;
using ProviderService.Services;
using ProviderService.Controllers;
using ProviderService.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add AWS Lambda support. When running the application as an AWS Serverless application, Kestrel is replaced
// with a Lambda function contained in the Amazon.Lambda.AspNetCoreServer package, which marshals the request into the ASP.NET Core hosting framework.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

// Add API Explorer and Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Logger
builder.Logging
        .ClearProviders()
        .AddJsonConsole();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Add services to the container.
builder.Services
        .AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

string region = Environment.GetEnvironmentVariable("AWS_REGION") ?? RegionEndpoint.USEast2.SystemName;
builder.Services.AddSingleton<IAmazonDynamoDB>(new AmazonDynamoDBClient(RegionEndpoint.GetBySystemName(region)));
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();

builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
builder.Services.AddScoped<IProviderServices, ProviderServices>();

builder.Services.AddScoped<IProviderLocationRepository, ProviderLocationRepository>();
builder.Services.AddScoped<IProviderLocationServices, ProviderLocationServices>();

builder.Services.AddScoped<IProviderPaymentMethodRepository, ProviderPaymentMethodRepository>(); 
builder.Services.AddScoped<IProviderPaymentMethodServices, ProviderPaymentMethodServices>();

builder.Services.AddScoped<IProviderContactRepository, ProviderContactRepository>();
builder.Services.AddScoped<IProviderContactServices, ProviderContactServices>();

builder.Services.AddScoped<IProviderAgreementRepository, ProviderAgreementRepository>();
builder.Services.AddScoped<IProviderAgreementServices, ProviderAgreementServices>(); 

builder.Services.AddCors();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options =>
{
    options.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Provider Lambda CRUD 100% Health");

app.MapProviderGetEndpoints();
app.MapProviderLocationEndpoints();

app.MapProviderPaymentMethodCreatedDtoEndpoints();

app.MapProviderContactGetDtoEndpoints();

app.MapProviderAgreementCreatedDtoEndpoints();

app.Run();
