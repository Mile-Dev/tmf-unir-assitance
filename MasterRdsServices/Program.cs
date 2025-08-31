using MasterRdsServices.Infraestructura.DataAccess.Common;
using MasterRdsServices.Infraestructura.DataAccess.Dao;
using MasterRdsServices.Infraestructura.DataAccess.Interface.EntitiesDao;
using MasterRdsServices.Services;
using Microsoft.EntityFrameworkCore;
using MasterRdsServices.Controllers;
using StorageS3Services.Common.Interfaces;
using Amazon.S3;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using MasterRdsServices.Infraestructura.DataAccesDynamo.Interfaces;
using MasterRdsServices.Infraestructura.DataAccesDynamo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Configurar el cliente de Amazon S3 utilizando las variables de configuración
var awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();

// Registrar AWS S3 como servicio
var s3Client = awsOptions.CreateServiceClient<IAmazonS3>();
builder.Services.AddSingleton<IAmazonS3>(s3Client);

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

//Add Repositories
builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddScoped<IEventProviderStatusRepository, EventProviderStatusRepository>();
builder.Services.AddScoped<IEventStatusRepository, EventStatusRepository>();
builder.Services.AddScoped<IGeneralTypesRepository, GeneralTypesRepository>();
builder.Services.AddScoped<IVoucherStatusRepository, VoucherStatusRepository>();
builder.Services.AddScoped<ICountriesAndCitiesRepository, CountriesAndCitiesRepository>();

//Add Services
builder.Services.AddScoped<ICategoriesServices, CategoriesServices>();
builder.Services.AddScoped<IGeneralTypesServices, GeneralTypesServices>();
builder.Services.AddScoped<IEventStatusServices, EventStatusServices>();
builder.Services.AddScoped<IVoucherStatusServices, VoucherStatusServices>();
builder.Services.AddScoped<IEventProviderStatusServices, EventProviderStatusServices>();
builder.Services.AddScoped<IProviderServices, ProviderServices>();
builder.Services.AddScoped<IS3Service, S3Service>();
builder.Services.AddScoped<IIcdServices, IcdServices>();
builder.Services.AddScoped<ICountriesAndCitiesServices, CountriesAndCitiesServices>();


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda");

app.MapCategoriesQueryDtoEndpoints();

app.MapGeneralTypesQueryDtoEndpoints();

app.MapEventProviderStatusQueryDtoEndpoints();

app.MapEventStatusQueryDtoEndpoints();

app.MapVoucherStatusQueryDtoEndpoints();

app.MapLocationEndpoints();

app.Run();
