using System;
using Biblio.Citations.Domain.CitationDomain.Models;

namespace Biblio.Citations.Domain.CitationDomain.Events;

/// <summary>
/// Event raised when a new citation is added to the domain.
/// </summary>
/// <remarks>
/// Contains the citation identifier, the citation text and the
/// moment when the event occurred. Implementations are immutable
/// (init-only) and intended to be used as domain events.
/// </remarks>
public sealed class CitationAddedEvent : ICitationEvent
{
    /// <summary>
    /// Gets the identifier of the citation that was added.
    /// </summary>
    public required CitationId CitationId { get; init; }

    /// <summary>
    /// Gets the textual content of the citation.
    /// </summary>
    public required string Text { get; init; }

    /// <summary>
    /// Gets the timestamp when the event occurred (UTC-aware).
    /// </summary>
    public required DateTimeOffset OccurredOn { get; init; }
}