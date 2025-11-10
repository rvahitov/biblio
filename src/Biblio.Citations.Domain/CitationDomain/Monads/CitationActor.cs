using Biblio.Citations.Domain.CitationDomain.Actors;
using Biblio.Citations.Domain.CitationDomain.Commands;
using Biblio.Citations.Domain.CitationDomain.Models;
using Biblio.Citations.Domain.CitationDomain.Queries;
using Biblio.Common.Akka;
using LanguageExt;
using LanguageExt.Traits;

namespace Biblio.Citations.Domain.CitationDomain.Monads;

/// <summary>
/// Monadic helpers for interacting with the <see cref="Biblio.Citations.Domain.CitationDomain.Actors.CitationActor"/>
/// from within a higher-kinded monad <typeparamref name="M"/> that provides an <see cref="IActorProvider"/> instance
/// and supports IO operations via <see cref="MonadIO{M}"/>.
/// </summary>
/// <typeparam name="M">The monad type which must implement <see cref="Has{M,T}"/> for <see cref="IActorProvider"/> and <see cref="MonadIO{M}"/>.</typeparam>
public static class CitationActor<M> where M : Has<M, IActorProvider>, MonadIO<M>
{
    /// <summary>
    /// Sends the given <see cref="ICitationCommand"/> to the citation actor and returns a monadic
    /// computation representing the command execution.
    /// </summary>
    /// <param name="command">The command to execute against the citation actor.</param>
    /// <returns>
    /// A <see cref="K{M,T}"/> representing a monadic computation which yields <see cref="Unit"/> on success.
    /// </returns>
    public static K<M, Unit> ExecuteCommand(ICitationCommand command) =>
        Actor<CitationActor, M>.Ask(command);

    /// <summary>
    /// Requests a citation by its identifier from the citation actor and returns a monadic computation
    /// that yields the requested <see cref="Citation"/>.
    /// </summary>
    /// <param name="citationId">The identifier of the citation to retrieve.</param>
    /// <returns>
    /// A <see cref="K{M,T}"/> representing a monadic computation which yields the requested <see cref="Citation"/>.
    /// </returns>
    public static K<M, Citation> GetCitation(CitationId citationId) =>
        Actor<CitationActor, M>.Ask(new GetCitationQuery { CitationId = citationId });
}
