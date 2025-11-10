using Biblio.Citations.Domain.CitationDomain.Commands;
using Biblio.Citations.Domain.CitationDomain.Events;
using Biblio.Citations.Domain.CitationDomain.Models;
using Biblio.Citations.Domain.CitationDomain.Monads;
using Biblio.Citations.Domain.CitationDomain.Queries;
using Biblio.Common;
using LanguageExt;

namespace Biblio.Citations.Domain.CitationDomain.Actors;

/// <summary>
/// Represents the persisted state of a citation actor in the citation workflow.
/// Contains the current <see cref="Citation"/> when present and provides methods
/// to process commands and queries against the citation workflow.
/// </summary>
/// <param name="CitationOrNone">Optional current citation stored in the state.</param>
/// <remarks>
/// This record implements persistence and query processing interfaces used by the
/// actor workflow and is intentionally immutable — state transitions are expressed
/// by returning new <see cref="CitationState"/> instances.
/// </remarks>
public sealed record CitationState(Option<Citation> CitationOrNone) :
IPersistableState<CitationState, ICitationWorkflowEnvironment, ICitationCommand, ICitationEvent, Unit>,
IProcessQuery<CitationState, ICitationWorkflowEnvironment, GetCitationQuery, Citation>,
ICitationWorkflowEnvironment
{
    /// <summary>
    /// Gets the initial, empty citation state (no citation present).
    /// </summary>
    public static CitationState Initial => new(Prelude.None);

    /// <summary>
    /// Gets the current citation, if any.
    /// </summary>
    public Option<Citation> Citation => CitationOrNone;

    /// <summary>
    /// Processes the given command in the context of the citation workflow and
    /// returns an effect that yields the sequence of events produced and a result marker.
    /// </summary>
    /// <param name="self">The current <see cref="CitationState"/> instance (not modified).</param>
    /// <param name="command">The command to process.</param>
    /// <returns>
    /// An <see cref="Eff{R,T}"/> representing a workflow effect which, when executed,
    /// yields a tuple containing the produced events and a <see cref="Unit"/> result.
    /// </returns>
    public static Eff<ICitationWorkflowEnvironment, (Seq<ICitationEvent> Events, Unit Result)> ProcessCommand(
        CitationState self,
        ICitationCommand command
    ) =>
        from events in CitationWorkflow.RunCommand(command)
        select (events, Unit.Default);

    /// <summary>
    /// Applies the provided event to this state and returns the new resulting state.
    /// </summary>
    /// <param name="event">The event to apply.</param>
    /// <returns>The new <see cref="CitationState"/> after the event has been applied.</returns>
    public CitationState ApplyEvent(ICitationEvent @event) => @event switch
    {
        CitationAddedEvent evt => ApplyEvent(evt),
        CitationTagAddedEvent evt => ApplyEvent(evt),
        CitationMetaAddedEvent evt => ApplyEvent(evt),
        _ => this
    };

    private CitationState ApplyEvent(CitationAddedEvent evt)
    {
        var citation = new Citation(
            evt.CitationId,
            evt.Text,
            CitationMetaCollection.Empty,
            CitationTagCollection.Empty
        );
        return new CitationState(citation);
    }

    private CitationState ApplyEvent(CitationTagAddedEvent evt) =>
        new(CitationOrNone.Map(citation => citation.AddTag(evt.Tag)));

    private CitationState ApplyEvent(CitationMetaAddedEvent evt) =>
        new(CitationOrNone.Map(citation => citation.AddMeta(evt.Meta)));

    /// <summary>
    /// Processes the provided <see cref="GetCitationQuery"/> and returns an effect
    /// that yields the requested <see cref="Citation"/>.
    /// </summary>
    /// <param name="self">The current <see cref="CitationState"/> instance (not modified).</param>
    /// <param name="query">The query to process.</param>
    /// <returns>An <see cref="Eff{R,T}"/> which, when executed, yields the requested <see cref="Citation"/>.</returns>
    public static Eff<ICitationWorkflowEnvironment, Citation> ProcessQuery(CitationState self, GetCitationQuery query) =>
        CitationWorkflow.ProcessQuery(query);
}
