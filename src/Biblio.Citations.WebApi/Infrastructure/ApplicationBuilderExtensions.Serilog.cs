using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Biblio.Citations.Infrastructure;

/// <summary>
/// Provides extension methods for application's builder.
/// </summary>
public static partial class ApplicationBuilderExtensions
{
    /// <summary>
    /// Configures Serilog for the provided <see cref="IHostApplicationBuilder"/>.
    /// This method will load optional settings from a <c>serilog.json</c> file (if present)
    /// and register Serilog services with dependency injection so that Serilog can
    /// read configuration and services from the host.
    /// </summary>
    /// <param name="appBuilder">The host application builder to configure.</param>
    /// <returns>The same <see cref="IHostApplicationBuilder"/> instance to allow fluent configuration.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="appBuilder"/> is <c>null</c>.</exception>
    public static IHostApplicationBuilder WithSerilog(this IHostApplicationBuilder appBuilder)
    {
        ArgumentNullException.ThrowIfNull(appBuilder);
        appBuilder.Configuration.AddJsonFile("serilog.json", optional: true, reloadOnChange: true);
        appBuilder.Services.AddSerilog((service, serilog) =>
        {
            var configuration = service.GetRequiredService<IConfiguration>();
            serilog.ReadFrom.Configuration(configuration);
            serilog.ReadFrom.Services(service);
        });
        return appBuilder;
    }
}
