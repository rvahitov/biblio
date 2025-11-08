using LanguageExt;

namespace Biblio.Citations.Domain.Common.Monads;

public sealed record WorkflowState(Seq<IDomainEvent> Events)
{
    public static readonly WorkflowState Empty = new([]);

    public WorkflowState AddEvent(IDomainEvent domainEvent) => new(Events.Add(domainEvent));

    public WorkflowState AddEvents(Seq<IDomainEvent> domainEvents) => new(Events + domainEvents);
}