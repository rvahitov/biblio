using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;

namespace Biblio.Citations.Infrastructure;

public static partial class ApplicationBuilderExtensions
{
    /// <summary>
    /// Configures FastEndpoints and Swagger for the provided host application builder.
    /// Adds FastEndpoints services and registers a Swagger document with the API title
    /// "Biblio Citations API" and version "v1".
    /// </summary>
    /// <param name="appBuilder">The host application builder whose services will be configured.</param>
    /// <returns>The same <see cref="IHostApplicationBuilder"/> instance to allow fluent chaining.</returns>
    /// <remarks>
    /// This extension method mutates <see cref="IHostApplicationBuilder.Services"/> by adding
    /// FastEndpoints and configuring Swagger document settings. Call this during host
    /// configuration before building the application.
    /// </remarks>
    public static IHostApplicationBuilder WithFastEndpoint(this IHostApplicationBuilder appBuilder)
    {
        appBuilder.Services
            .AddFastEndpoints()
            .SwaggerDocument(swagger =>
            {
                swagger.AutoTagPathSegmentIndex = 2; // Use the 2nd segment of the path as the tag
                swagger.NewtonsoftSettings = json =>
                {
                    json.Converters.Add(new StringEnumConverter()); // Serialize enums as strings
                };
                swagger.DocumentSettings = doc =>
                {
                    doc.Title = "Biblio Citations API";
                    doc.Version = "v1";
                };
            });
        return appBuilder;
    }
}
