using Akka.Actor;
using LanguageExt;

namespace Biblio.Common.Akka;

/// <summary>
/// Marker/placeholder interface for providing actor references or actor system access.
/// Implementations will be added as the Akka integration grows.
/// </summary>
public interface IActorProvider
{
    /// <summary>
    /// Get an actor or service by the requested type.
    /// Implementations should resolve or create the actor and return it wrapped in an IO for lazy/effectful access.
    /// </summary>
    /// <typeparam name="A">The expected type of the actor or adapter to be returned (for example an actor adapter or <c>IActorRef</c>).</typeparam>
    /// <returns>
    /// A <c>LanguageExt.IO</c> containing an <c>IActorRef</c> that represents the requested actor.
    /// The IO allows deferred retrieval and execution semantics consistent with the project's functional patterns.
    /// </returns>
    /// <remarks>
    /// The generic type parameter <typeparamref name="A"/> is provided to express intent;
    /// concrete implementations may use it to select an appropriate actor or adapter instance.
    /// </remarks>
    IO<IActorRef> GetActor<A>();
}