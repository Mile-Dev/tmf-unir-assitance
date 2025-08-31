using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using IssuanceMokServices.Domain.Interfaces;
using IssuanceMokServices.Infraestructure.DataAccess;
using Amazon.S3;
using StorageS3Services.Common.Interfaces;
using IssuanceMokServices.Controllers;
using IssuanceMokServices.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
                .AddNewtonsoftJson();

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

// Configurar el cliente de Amazon S3 utilizando las variables de configuración
var awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();

// Registrar AWS S3 como servicio
var s3Client = awsOptions.CreateServiceClient<IAmazonS3>();
builder.Services.AddSingleton<IAmazonS3>(s3Client);

// Registrar el servicio de S3 en la inyeccion de dependencias
builder.Services.AddScoped<IS3Service, S3Service>();

// Registrar el servicio de Services.
builder.Services.AddScoped<IIssuanceMokServices, IssuanceMokServices.Services.IssuanceMokServices>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Registrar el servicio de Repository.
builder.Services.AddScoped<IIssuanceMokRepository, IssuanceMokRepository>();

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

app.MapUploadRequestEndpoints();

app.Run();
