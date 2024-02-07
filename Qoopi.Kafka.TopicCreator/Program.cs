using System.Reflection;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Exceptions;

var builder = WebApplication.CreateBuilder(args);
    builder.Configuration
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", true, true)
        .AddEnvironmentVariables();
var LoggerConfiguration = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithExceptionDetails();

Log.Logger = LoggerConfiguration.CreateLogger();

    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(Log.Logger);
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(opt =>
    {
        var xml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var path = Path.Combine(AppContext.BaseDirectory, xml);
        opt.IncludeXmlComments(path);
        opt.UseInlineDefinitionsForEnums();
        opt.SwaggerDoc("v1.0.0", new OpenApiInfo
        {
            Title = "Topic Creator v1.0.0",
            Description = "Qoopi's servcie to create topics in Kafka.</br> Author: Oleh Kutafin"
        });
    });

var app = builder.Build();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
    app.MapGet("/weatherforecast", () => { })
        .WithName("GetWeatherForecast")
        .WithOpenApi();
    app.Run();
