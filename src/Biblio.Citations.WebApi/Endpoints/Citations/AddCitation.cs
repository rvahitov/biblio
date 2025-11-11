using System.Threading;
using System.Threading.Tasks;
using Biblio.Citations.Endpoints.Citations.DTO;
using Biblio.Citations.Services;
using FastEndpoints;
using LanguageExt;
using Microsoft.AspNetCore.Builder;

namespace Biblio.Citations.Endpoints.Citations;

/// <summary>
/// Endpoint that handles creation of a new citation.
/// Accepts an <see cref="AddCitationRequest"/> and returns an <see cref="AddCitationResponse"/> containing the created citation identifier.
/// </summary>
/// <param name="appEnvironment">Application environment providing configuration and dependencies required to run the endpoint.</param>
public sealed class AddCitation(AppEnvironment appEnvironment) : Ep.Req<AddCitationRequest>.Res<AddCitationResponse>
{
    /// <summary>
    /// Configure the endpoint route, access rules and OpenAPI metadata.
    /// </summary>
    public override void Configure()
    {
        Post("/api/citations");
        AllowAnonymous();
        Summary(new AddCitationSummary());
        Description(route => route.WithName("AddCitation"));
    }

    /// <summary>
    /// Handles the incoming add-citation request.
    /// The request is validated upstream by FastEndpoints validation rules. The service is invoked to persist
    /// the citation and a response DTO is produced.
    /// </summary>
    /// <param name="req">The request DTO containing the citation data.</param>
    /// <param name="ct">Cancellation token forwarded to underlying operations.</param>
    /// <returns>A task which completes when the response is sent.</returns>
    public override async Task HandleAsync(AddCitationRequest req, CancellationToken ct)
    {
        var response = await CitationService
            .AddCitation(req)
            .Run(appEnvironment)
            .RunAsync(EnvIO.New(token: ct));

        // Note: the service returns a response DTO (or a monadic result). This endpoint currently
        // returns 200 OK with the created DTO. Consider returning 201 Created when a resource is created.
        await Send.OkAsync(response, ct);
    }
}

public sealed class AddCitationSummary : EndpointSummary<AddCitation>
{
    private static readonly AddCitationResponse ResponseExample = new()
    {
        CitationId = "123e4567-e89b-12d3-a456-426614174000",
    };
    public AddCitationSummary()
    {
        Summary = "Adds a new citation to the system.";
        Description = "Adds a new citation using the provided details (text, author, source and optional metadata). " +
                      "The request is validated, the citation is persisted, and the API returns the created citation's identifier. " +
                      "On success the endpoint returns 200 OK; on validation or processing errors it returns an appropriate error response.";
        Response(200, "Citation added successfully.", example: ResponseExample);
    }
}