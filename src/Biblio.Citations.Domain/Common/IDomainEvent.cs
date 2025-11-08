using System;

namespace Biblio.Citations.Domain.Common;

/// <summary>
/// Represents a domain event produced by the Biblio domain model.
/// Domain events capture facts about something that has already happened
/// and are typically immutable (read-only) data carriers used for integration
/// and causal tracking.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// The date and time at which the event occurred.
    /// </summary>
    /// <value>
    /// A <see cref="DateTimeOffset"/> representing when the event took place.
    /// The value is offset-aware and should be treated as the canonical timestamp
    /// for the event occurrence.
    /// </value>
    DateTimeOffset OccurredOn { get; }
}
