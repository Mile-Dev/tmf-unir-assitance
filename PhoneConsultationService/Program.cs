using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using PhoneConsultationService.Api;
using PhoneConsultationService.DataAccess;
using PhoneConsultationService.Domain.Interfaces;
using PhoneConsultationService.Infraestructure.DataCie11;
using PhoneConsultationService.Services;
using Serilog;
using StorageS3Services.Common.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Variables de entorno inyectadas por CDK en la Lambda:
var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "DEV";   
var serviceName = Environment.GetEnvironmentVariable("SERVICE_NAME") ?? "PhoneConsultationService";
var regionName = Environment.GetEnvironmentVariable("AWS_REGION") ?? "us-east-1";      
var prefix = Environment.GetEnvironmentVariable("CONFIG_PREFIX") ?? $"/tw/{envName}/{serviceName}/";

// 1) Config base opcional (solo útil en local) + env vars
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables();


// Configure AWS Lambda Hosting
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

// Add API Explorer and Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure AWS options
var awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();
builder.Services.AddAWSService<IAmazonS3>();

// Registrar el servicio de S3 en la inyeccion de dependencias
builder.Services.AddScoped<IS3Service, S3Service>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Configure Serilog from appsettings.json
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// Add repositories and services
builder.Services.AddScoped<ICid11ApiClientRepository, Cid11ApiClientRepository>();
builder.Services.AddScoped<Cid11ApiClientServices>();
builder.Services.AddScoped<IPhoneConsultationRepository, PhoneConsultationRepository>();
builder.Services.AddScoped<PhoneConsultationServices>();
builder.Services.AddScoped<IStorageS3Services, PhoneConsultationService.Services.StorageS3Services>();

builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS
app.UseCors(options =>
{
    options.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

// Configure endpoints
app.PhoneConsultationEndpoints();
app.Cie11Endpoints();

app.MapS3ServiceEndpoints();

app.Run();
