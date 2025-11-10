using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;

namespace Biblio.Citations.Endpoints;

/// <summary>
/// Represents a simple ping endpoint that returns the current UTC time.
/// This endpoint is used for health checks and basic connectivity testing.
/// </summary>
public sealed class Ping : Ep.NoReq.Res<PingResponse>
{
    /// <summary>
    /// Configures the endpoint settings, such as route, HTTP method, and permissions.
    /// </summary>
    public override void Configure()
    {
        Get("/api/ping");
        AllowAnonymous();
        // Description: Returns the current UTC time for health check purposes
    }

    ///<inheritdoc/>
    public override Task HandleAsync(CancellationToken ct)
    {
        var response = new PingResponse(DateTimeOffset.UtcNow);
        return Send.OkAsync(response, ct);
    }
}

public sealed record PingResponse(DateTimeOffset UtcNow);
