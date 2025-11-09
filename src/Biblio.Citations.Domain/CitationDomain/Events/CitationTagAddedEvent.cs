using System;
using Biblio.Citations.Domain.CitationDomain.Models;

namespace Biblio.Citations.Domain.CitationDomain.Events;

/// <summary>
/// Event raised when a tag is added to a citation.
/// </summary>
/// <remarks>
/// Contains the citation identifier, the added tag text and the
/// moment when the tagging occurred. Instances are immutable
/// (init-only) and represent domain events that can be published
/// to event handlers or persisted to an event store.
/// </remarks>
public sealed class CitationTagAddedEvent : ICitationEvent
{
    /// <summary>
    /// Gets the identifier of the citation which received the tag.
    /// </summary>
    public required CitationId CitationId { get; init; }

    /// <summary>
    /// Gets the tag that was added to the citation.
    /// </summary>
    public required string Tag { get; init; }

    /// <summary>
    /// Gets the timestamp when the tag was added (UTC-aware).
    /// </summary>
    public required DateTimeOffset OccurredOn { get; init; }
}