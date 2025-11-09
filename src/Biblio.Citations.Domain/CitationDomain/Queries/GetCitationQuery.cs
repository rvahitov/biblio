using Biblio.Citations.Domain.CitationDomain.Models;
using Biblio.Common.Messages;

namespace Biblio.Citations.Domain.CitationDomain.Queries;

/// <summary>
/// Query for retrieving a single <see cref="Citation"/> by its identifier.
/// </summary>
/// <remarks>
/// Handled by a query handler that returns a successful <see cref="Citation"/> or a failure.
/// Implements <see cref="IFallibleQuery{T}"/> to indicate the query may fail (for example,
/// when the citation with the specified id does not exist).
/// </remarks>
public sealed class GetCitationQuery : ICitationQuery, IFallibleQuery<Citation>
{
    /// <summary>
    /// Identifier of the citation to retrieve.
    /// This property is required and should be provided by the caller.
    /// </summary>
    public required CitationId CitationId { get; init; }
}
