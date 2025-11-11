using System;
using Akka.Actor;
using Akka.Hosting;
using Biblio.Common.Akka;
using LanguageExt;

namespace Biblio.Citations.Services;

/// <summary>
/// Provides actor references resolved from an <see cref="IReadOnlyActorRegistry"/>
/// wrapped as a LanguageExt <c>IO</c> effect.
/// </summary>
public sealed class ActorRegistryProvider : IActorProvider
{
    private readonly IReadOnlyActorRegistry _actorRegistry;

    /// <summary>
    /// Initializes a new instance of <see cref="ActorRegistryProvider"/>.
    /// </summary>
    /// <param name="actorRegistry">The actor registry used to resolve actor references.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="actorRegistry"/> is <c>null</c>.</exception>
    public ActorRegistryProvider(IReadOnlyActorRegistry actorRegistry)
    {
        ArgumentNullException.ThrowIfNull(actorRegistry);
        _actorRegistry = actorRegistry;
    }

    /// <summary>
    /// Returns an <c>IO&lt;IActorRef&gt;</c> effect that resolves an actor reference for the requested actor type.
    /// </summary>
    /// <typeparam name="A">Actor type to resolve.</typeparam>
    /// <returns>An <c>IO&lt;IActorRef&gt;</c> that, when executed, resolves the actor reference.</returns>
    /// <remarks>
    /// The underlying registry lookup receives the <see cref="System.Threading.CancellationToken"/>
    /// provided by the LanguageExt <c>IO</c> environment when the effect is executed.
    /// </remarks>
    public IO<IActorRef> GetActor<A>() => IO.liftAsync(e => _actorRegistry.GetAsync<A>(e.Token));
}
