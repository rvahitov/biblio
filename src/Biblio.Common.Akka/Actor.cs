using Akka.Actor;
using Biblio.Common.Messages;
using LanguageExt;
using LanguageExt.Traits;

namespace Biblio.Common.Akka;

/// <summary>
/// Static helper for interacting with an actor resolved by the type parameter <typeparamref name="A"/>.
/// The helper runs in an ambient monadic context <typeparamref name="M"/> which must provide an
/// <see cref="IActorProvider"/> (via <see cref="Has{M,T}"/>) and support IO operations (via <see cref="MonadIO{M}"/>).
/// </summary>
/// <typeparam name="A">The actor or adapter type used to locate or identify the desired actor instance.</typeparam>
/// <typeparam name="M">The monadic effect type that supplies an <see cref="IActorProvider"/> and supports IO semantics.</typeparam>
public static class Actor<A, M> where M : Has<M, IActorProvider>, MonadIO<M>
{
    /// <summary>
    /// Monadic computation that resolves the <see cref="IActorRef"/> for the given actor key <typeparamref name="A"/>.
    /// </summary>
    private static readonly K<M, IActorRef> GetActor =
        from provider in M.Ask
        from actor in provider.GetActor<A>()
        select actor;

    /// <summary>
    /// Sends a fire-and-forget message to the resolved actor.
    /// </summary>
    /// <param name="message">The message to send to the actor. Can be any object the actor understands.</param>
    /// <returns>
    /// A monadic computation that yields <see cref="Unit"/> when the tell operation has been invoked.
    /// The actual sending is performed inside an IO lift to preserve effectful semantics.
    /// </returns>
    public static K<M, Unit> Tell(object message) =>
        from actor in GetActor
        from _ in IO.lift(() => actor.Tell(message))
        select Unit.Default;

    /// <summary>
    /// Sends a request message to the actor and returns a typed response inside the monad.
    /// </summary>
    /// <typeparam name="TResponse">The expected response type.</typeparam>
    /// <param name="message">A request message implementing <see cref="IMessage{TResponse}"/>.</param>
    /// <returns>
    /// A monadic computation that yields the typed response when the underlying Ask completes.
    /// </returns>
    public static K<M, TResponse> Ask<TResponse>(IMessage<TResponse> message) =>
        from actor in GetActor
        from response in IO.liftAsync(e => actor.Ask<TResponse>(message, e.Token))
        select response;

    /// <summary>
    /// Sends a fallible request message to the actor and converts the <see cref="Fin{T}"/> result into the concrete response type.
    /// </summary>
    /// <typeparam name="TResponse">The expected inner response type.</typeparam>
    /// <param name="message">A request message implementing <see cref="IFallibleMessage{TResponse}"/>.</param>
    /// <returns>
    /// A monadic computation that yields the response when the underlying <see cref="Fin{TResponse}"/> is successful,
    /// otherwise the failure is propagated according to the semantics of <see cref="Fin{T}"/> and the surrounding monad.
    /// </returns>
    public static K<M, TResponse> Ask<TResponse>(IFallibleMessage<TResponse> message) =>
        from fin in Ask<Fin<TResponse>>(message)
        from response in IO.lift(fin)
        select response;
}
