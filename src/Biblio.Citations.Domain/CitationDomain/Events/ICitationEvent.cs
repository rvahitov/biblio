using Biblio.Citations.Domain.CitationDomain.Models;
using Biblio.Citations.Domain.Common;

namespace Biblio.Citations.Domain.CitationDomain.Events;

/// <summary>
/// Represents a domain event related to a citation.
/// </summary>
/// <remarks>
/// Implementations contain the data required by the domain to react to
/// changes associated with a <see cref="CitationId"/>. This interface
/// extends <see cref="IDomainEvent"/> to mark events that concern a
/// specific citation.
/// </remarks>
public interface ICitationEvent : IDomainEvent
{
    /// <summary>
    /// Gets the identifier of the citation this event concerns.
    /// </summary>
    CitationId CitationId { get; }
}
