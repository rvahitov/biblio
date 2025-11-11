using System;
using Biblio.Common.Akka;
using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Traits;

namespace Biblio.Citations.Services.AppMonad;

/// <summary>
/// The <c>App</c> kind that provides derived instances and typeclass implementations
/// for the application monad used by the WebApi services.
///
/// This class wires up the various LanguageExt derivations (monad transformer, readable,
/// choice), plus helper typeclasses (MonadIO, Fallible) and the environment-has pattern
/// for <see cref="IActorProvider"/>.
/// </summary>
public abstract class App :
    Deriving<App, ReaderT<AppEnvironment, IO>>,
    Deriving.MonadT<App, ReaderT<AppEnvironment, IO>, IO>,
    Deriving.Readable<App, AppEnvironment, ReaderT<AppEnvironment, IO>>,
    Deriving.Choice<App, ReaderT<AppEnvironment, IO>>,
    MonadIO<App>,
    Fallible<App>,
    Has<App, IActorProvider>
{
    /// <summary>
    /// The unit value for the <c>App</c> kind.
    /// </summary>
    public static App<Unit> UnitM { get; } = new(ReaderT<AppEnvironment, IO, Unit>.Pure(Unit.Default));

    /// <summary>
    /// Co-natural transformation from the underlying ReaderT representation into the
    /// <c>App</c> kind. Used by LanguageExt's deriving mechanics to construct
    /// <c>K&lt;App, A&gt;</c> values from <c>K&lt;ReaderT&lt;AppEnvironment, IO&gt;, A&gt;</c>.
    /// </summary>
    /// <typeparam name="A">Result type for the computation.</typeparam>
    /// <param name="fa">The ReaderT-based computation to lift into <c>App</c>.</param>
    /// <returns>An <c>App&lt;A&gt;</c> wrapping the provided <c>ReaderT</c> computation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="fa"/> is <c>null</c>.</exception>
    static K<App, A> CoNatural<App, ReaderT<AppEnvironment, IO>>.CoTransform<A>(K<ReaderT<AppEnvironment, IO>, A> fa)
    {
        ArgumentNullException.ThrowIfNull(fa);
        return new App<A>(fa);
    }

    /// <summary>
    /// Natural transformation from the <c>App</c> kind into the underlying ReaderT representation.
    /// This extracts the <see cref="App{A}.RunApp"/> computation from an <c>App&lt;A&gt;</c> value.
    /// </summary>
    /// <typeparam name="A">Result type for the computation.</typeparam>
    /// <param name="fa">The <c>App&lt;A&gt;</c> value to transform.</param>
    /// <returns>The underlying <c>ReaderT&lt;AppEnvironment, IO, A&gt;</c> computation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="fa"/> is <c>null</c>.</exception>
    static K<ReaderT<AppEnvironment, IO>, A> Natural<App, ReaderT<AppEnvironment, IO>>.Transform<A>(K<App, A> fa)
    {
        ArgumentNullException.ThrowIfNull(fa);
        return ((App<A>)fa).RunApp;
    }

    /// <summary>
    /// MonadIO implementation: lift an <see cref="IO{T}"/> into the <c>App</c> kind.
    /// </summary>
    /// <typeparam name="A">Result type of the IO effect.</typeparam>
    /// <param name="ma">IO effect to lift.</param>
    /// <returns>An <c>App&lt;A&gt;</c> representing the lifted IO.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="ma"/> is <c>null</c>.</exception>
    static K<App, A> MonadIO<App>.LiftIO<A>(IO<A> ma)
    {
        ArgumentNullException.ThrowIfNull(ma);
        return App<A>.LiftIO(ma);
    }

    /// <summary>
    /// Fallible implementation: construct a failing computation from a LanguageExt <see cref="LanguageExt.Common.Error"/>.
    /// </summary>
    /// <typeparam name="A">Result type of the failed computation.</typeparam>
    /// <param name="error">The error to fail with.</param>
    /// <returns>An <c>App&lt;A&gt;</c> representing the failure.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="error"/> is <c>null</c>.</exception>
    static K<App, A> Fallible<Error, App>.Fail<A>(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);
        return App<A>.LiftIO(IO<A>.Fail(error));
    }

    /// <summary>
    /// Fallible Catch implementation: catch errors in the underlying IO and map them
    /// back into the <c>App</c> kind using the provided <paramref name="Fail"/> function.
    /// </summary>
    /// <typeparam name="A">Result type of the computation.</typeparam>
    /// <param name="fa">Computation to run and potentially catch errors from.</param>
    /// <param name="Predicate">Predicate to select which errors to handle.</param>
    /// <param name="Fail">Mapping from <see cref="LanguageExt.Common.Error"/> to a failure computation.</param>
    /// <returns>An <c>App&lt;A&gt;</c> that runs <paramref name="fa"/> and applies the handler on errors.</returns>
    /// <exception cref="ArgumentNullException">Thrown when any argument is <c>null</c>.</exception>
    static K<App, A> Fallible<Error, App>.Catch<A>(K<App, A> fa, Func<Error, bool> Predicate, Func<Error, K<App, A>> Fail)
    {
        ArgumentNullException.ThrowIfNull(fa);
        ArgumentNullException.ThrowIfNull(Predicate);
        ArgumentNullException.ThrowIfNull(Fail);
        var readerT = new ReaderT<AppEnvironment, IO, A>(env =>
        {
            var io = fa.As().Run(env);
            return io.Catch(Predicate, err => Fail(err).As().RunApp.Run(env));
        });
        return new App<A>(readerT);
    }

    /// <summary>
    /// Has&lt;IActorProvider&gt; implementation: asks the environment for the registered
    /// <see cref="IActorProvider"/> instance.
    /// </summary>
    static K<App, IActorProvider> Has<App, IActorProvider>.Ask =>
        Readable.asks<App, AppEnvironment, IActorProvider>(env => env.ActorProvider);
}
