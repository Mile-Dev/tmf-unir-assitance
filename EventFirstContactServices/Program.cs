using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using EventFirstContactServices.Controllers;
using EventFirstContactServices.Domain.Interfaces;
using EventFirstContactServices.Infraestructura.DataAccess;
using EventFirstContactServices.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// Configure AWS options
// Asume que la configuraci�n de AWS se carga de appsettings.json o variables de entorno
// se instala esta libreria  => dotnet add package AWSSDK.Extensions.NETCore.Setup
var awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();
builder.Services.AddScoped(typeof(IEventFirstContactRepository<>), typeof(EventFirstContactRepository<>));
builder.Services.AddScoped<IEventFirstContactServices, EventFirstContactServices.Services.EventFirstContactServices>();
builder.Services.AddScoped<IEventContactEmergencyContactServices, EventContactEmergencyContactServices>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

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

app.MapEventFirstContactCreateDtoEndpoints();

app.MapEventFirstContactEmergencyContactGetDtoEndpoints();

app.Run();
