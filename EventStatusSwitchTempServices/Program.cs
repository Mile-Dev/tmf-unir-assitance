using EventStatusSwitchTempServices.Infraestructura.DataAccess;
using EventStatusSwitchTempServices.Infraestructura.DataAccess.Dao;
using EventStatusSwitchTempServices.Infraestructura.Interface.EntitiesDao;
using EventStatusSwitchTempServices.Services;
using Microsoft.EntityFrameworkCore;
using EventStatusSwitchTempServices.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);


// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

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

builder.Services.AddScoped<IEventsRepository, EventsRepository>();
builder.Services.AddScoped<IEventStatusRepository, EventStatusRepository>();


//Add Services
builder.Services.AddScoped<IEventStatusSwitchServices, EventStatusSwitchServices>();

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

app.MapRequestUpdatedStatusEventEndpoints();

app.Run();
