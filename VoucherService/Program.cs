//using microservice_voucher.Domain.Dto;
using Serilog;
using VoucherService.Common.Configuration;
using VoucherService.Controllers;
using VoucherService.Infraestructura.EndpointSetw;
using VoucherService.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure AWS Lambda Hosting
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

// Add services to the container.
builder.Services.AddCors();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddSingleton<IConfigurationApplication, ConfigurationApplication>();
builder.Services.AddSingleton<IInvokeIntegrationSetwCoverage, InvokeIntegrationSetwCoverage>();
builder.Services.AddSingleton<ICoverageServices, CoverageServices>();


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

app.MapVoucherEndpoints();
app.MapCovergeEndpoints();

app.Run();
