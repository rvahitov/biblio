using System;
using Biblio.Citations.Domain.CitationDomain.Models;

namespace Biblio.Citations.Domain.CitationDomain.Events;

/// <summary>
/// Event raised when metadata is added to a citation.
/// </summary>
/// <remarks>
/// Contains the citation identifier, the <see cref="CitationMeta"/> entry
/// that was added, and the timestamp when the metadata was recorded.
/// Instances are immutable (init-only) and represent domain events
/// suitable for publishing or persisting to an event store.
/// </remarks>
public sealed class CitationMetaAddedEvent : ICitationEvent
{
    /// <summary>
    /// Gets the identifier of the citation that received the metadata.
    /// </summary>
    public required CitationId CitationId { get; init; }

    /// <summary>
    /// Gets the metadata entry that was added to the citation.
    /// </summary>
    public required CitationMeta Meta { get; init; }

    /// <summary>
    /// Gets the timestamp when the metadata was added (UTC-aware).
    /// </summary>
    public required DateTimeOffset OccurredOn { get; init; }
}