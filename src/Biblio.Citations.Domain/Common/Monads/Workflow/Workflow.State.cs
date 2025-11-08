using LanguageExt;

namespace Biblio.Citations.Domain.Common.Monads;

/// <summary>
/// Represents the mutable state carried by workflows. Currently this is a simple
/// container for the sequence of domain events produced during workflow execution.
/// </summary>
/// <param name="Events">The sequence of domain events recorded in the workflow state.</param>
public sealed record WorkflowState(Seq<IDomainEvent> Events)
{
    /// <summary>
    /// An empty workflow state with no recorded events.
    /// </summary>
    public static readonly WorkflowState Empty = new([]);

    /// <summary>
    /// Return a copy of this state with the supplied event appended.
    /// </summary>
    /// <param name="domainEvent">The domain event to append.</param>
    /// <returns>A new <see cref="WorkflowState"/> with the event appended.</returns>
    public WorkflowState AddEvent(IDomainEvent domainEvent) => new(Events.Add(domainEvent));

    /// <summary>
    /// Return a copy of this state with multiple events appended.
    /// </summary>
    /// <param name="domainEvents">Sequence of domain events to append.</param>
    /// <returns>A new <see cref="WorkflowState"/> with the events appended.</returns>
    public WorkflowState AddEvents(Seq<IDomainEvent> domainEvents) => new(Events + domainEvents);
}