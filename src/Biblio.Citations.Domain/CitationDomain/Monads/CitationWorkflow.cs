using System;
using Biblio.Citations.Domain.CitationDomain.Commands;
using Biblio.Citations.Domain.CitationDomain.Events;
using Biblio.Citations.Domain.CitationDomain.Models;
using Biblio.Citations.Domain.CitationDomain.Queries;
using Biblio.Citations.Domain.Common;
using Biblio.Citations.Domain.Common.Monads;
using Biblio.Common.Extensions;
using LanguageExt;
using LanguageExt.Common;

namespace Biblio.Citations.Domain.CitationDomain.Monads;

/// <summary>
/// Central workflow definitions for the citation domain.
///
/// This class composes small workflow primitives to implement command handling and
/// query evaluation for citations. Public APIs on this class expose Effects and
/// Workflow values that can be executed by the runtime using an
/// <see cref="ICitationWorkflowEnvironment"/>.
/// </summary>
public abstract class CitationWorkflow : Workflow
{
    private static readonly Workflow<Option<Citation>> Citation =
        AsksM<ICitationWorkflowEnvironment, Option<Citation>>(env => Pure(env.Citation));

    private static readonly Workflow<Citation> GetCitation =
        from option in Citation
        from citation in option.IsJust(out var c) ? Pure(c) : Fail<Citation>(Error.New("Citation not found"))
        select citation;

    private static Workflow<Unit> EnsureCitationNotExists() =>
        from option in Citation
        from _1 in option.IsNone ? Pure(Unit.Default) : Fail<Unit>(Error.New("Citation already exists"))
        select Unit.Default;

    /// <summary>
    /// Create a workflow that processes a domain command for citations.
    /// </summary>
    /// <param name="command">The command to process.</param>
    /// <returns>
    /// A <see cref="Workflow{Unit}"/> representing the command handling. The workflow
    /// either completes successfully (yielding <see cref="Unit"/>) or fails with an <see cref="Error"/>.
    /// </returns>
    public static Workflow<Unit> ProcessCommand(ICitationCommand command) => command switch
    {
        AddCitationCommand cmd => ProcessCommand(cmd),
        _ => Fail<Unit>(Error.New("Unknown command"))
    };

    private static Workflow<Unit> ProcessCommand(AddCitationCommand command) =>
        from _1 in EnsureCitationNotExists()
        from now in LiftIO(IO.lift(() => DateTimeOffset.Now))
        from _2 in AddEvent(new CitationAddedEvent
        {
            CitationId = command.CitationId,
            Text = command.Text,
            OccurredOn = now
        })
        from _3 in AddEvents(command.Tags.ToIterable().Map(tag => CreateTagAddedEvent(command.CitationId, tag, now))
            .ToSeq())
        from _4 in AddEvents(command.Meta.ToIterable().Map(m => CreateMetadataAddedEvent(command.CitationId, m, now))
            .ToSeq())
        select Unit.Default;

    /// <summary>
    /// Run a command as an effect using the runtime environment and return the
    /// sequence of domain events produced by handling the command.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <returns>
    /// An <see cref="Eff{R,T}"/> that, when evaluated with an
    /// <see cref="ICitationWorkflowEnvironment"/>, returns the produced
    /// sequence of <see cref="ICitationEvent"/> objects.
    /// </returns>
    public static Eff<ICitationWorkflowEnvironment, Seq<ICitationEvent>> RunCommand(ICitationCommand command) =>
        from env in Prelude.runtime<ICitationWorkflowEnvironment>()
        from state in ProcessCommand(command).Exec(env)
        select state.Events.Map(e => (ICitationEvent)e);

    /// <summary>
    /// Evaluate a read-only query against the citation workflow environment.
    /// </summary>
    /// <param name="query">The query to evaluate; currently supports <see cref="GetCitationQuery"/>.</param>
    /// <returns>
    /// An <see cref="Eff{R,T}"/> that, when executed with an
    /// <see cref="ICitationWorkflowEnvironment"/>, returns the requested
    /// <see cref="Citation"/> or fails if it is not found.
    /// </returns>
    public static Eff<ICitationWorkflowEnvironment, Citation> ProcessQuery(GetCitationQuery query) =>
        from env in Prelude.runtime<ICitationWorkflowEnvironment>()
        from citation in GetCitation.Eval(env)
        select citation;

    private static IDomainEvent CreateTagAddedEvent(CitationId citationId, string tag, DateTimeOffset occurredOn) =>
        new CitationTagAddedEvent
        {
            CitationId = citationId,
            Tag = tag,
            OccurredOn = occurredOn
        };

    private static IDomainEvent CreateMetadataAddedEvent(CitationId citationId, CitationMeta meta, DateTimeOffset occurredOn)
        => new CitationMetaAddedEvent
        {
            CitationId = citationId,
            Meta = meta,
            OccurredOn = occurredOn
        };
}
