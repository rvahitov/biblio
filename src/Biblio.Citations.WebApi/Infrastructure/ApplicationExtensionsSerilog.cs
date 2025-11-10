using Microsoft.AspNetCore.Builder;
using Serilog;

namespace Biblio.Citations.Infrastructure;

public static partial class ApplicationExtensions
{
    public static WebApplication WithSerilog(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        return app;
    }
}
