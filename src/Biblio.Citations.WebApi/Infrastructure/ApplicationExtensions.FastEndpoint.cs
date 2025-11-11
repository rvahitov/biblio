using System.Text.Json.Serialization;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;

namespace Biblio.Citations.Infrastructure;

public static partial class ApplicationExtensions
{
    public static WebApplication WithFastEndpoints(this WebApplication app)
    {
        app.UseFastEndpoints(fe =>
        {
            fe.Serializer.Options.Converters.Add(new JsonStringEnumConverter()); // Serialize enums as strings
            fe.Endpoints.ShortNames = true; // Use short names for endpoints in Swagger
        });
        app.UseSwaggerGen();
        app.UseReDoc(redoc =>
        {
            redoc.Path = "/redoc";
            redoc.DocumentTitle = "Biblio Citations API Documentation";
        }); // Serve ReDoc at /redoc
        return app;
    }
}
