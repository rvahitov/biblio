
using Biblio.Citations.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.OpenTelemetry()
    .CreateBootstrapLogger();

var builder = WebApplication
    .CreateBuilder(args)
    .AddServiceDefaults();

builder
    .WithSerilog() // Configure Serilog for the application
    .WithFastEndpoint() // Configure FastEndpoints for the application
    .WithAkka() // Configure Akka.NET for the application
;

// Add services to the container.

var app = builder
    .Build()
    .WithSerilog() // Use Serilog middleware
    .MapDefaultEndpoints() // Map default endpoints
    .WithFastEndpoints() // Use FastEndpoints middleware
    ;

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.Run();
