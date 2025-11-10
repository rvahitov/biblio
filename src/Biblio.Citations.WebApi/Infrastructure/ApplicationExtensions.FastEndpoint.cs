using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;

namespace Biblio.Citations.Infrastructure;

public static partial class ApplicationExtensions
{
    public static WebApplication WithFastEndpoints(this WebApplication app)
    {
        app.UseFastEndpoints();
        app.UseSwaggerGen();
        return app;
    }
}
