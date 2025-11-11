using System;
using LanguageExt;
using LanguageExt.Traits;

namespace Biblio.Citations.Services.AppMonad;

/// <summary>
/// Represents the application monad used across the WebApi services.
/// Internally this is a <see cref="ReaderT{TEnv, TInner}"/> over an <see cref="IO{T}"/>
/// where the environment is <see cref="AppEnvironment"/> and the inner effect is <see cref="IO{T}"/>.
/// </summary>
/// <typeparam name="A">The result type produced by the application computation.</typeparam>
public sealed class App<A> : K<App, A>
{
    /// <summary>
    /// Initializes a new instance of <see cref="App{A}"/> from a <c>K&lt;ReaderT&lt;AppEnvironment, IO&gt;, A&gt;</c>.
    /// </summary>
    /// <param name="runApp">The underlying computation to run, represented as a ReaderT over IO.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="runApp"/> is <c>null</c>.</exception>
    public App(K<ReaderT<AppEnvironment, IO>, A> runApp)
    {
        ArgumentNullException.ThrowIfNull(runApp);
        RunApp = runApp;
    }

    /// <summary>
    /// Gets the underlying ReaderT&lt;AppEnvironment, IO&gt; computation.
    /// </summary>
    public K<ReaderT<AppEnvironment, IO>, A> RunApp { get; }

    /// <summary>
    /// Lifts a plain <see cref="IO{A}"/> into the application monad.
    /// </summary>
    /// <param name="io">The IO effect to lift.</param>
    /// <returns>An <see cref="App{A}"/> representing the lifted IO effect.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="io"/> is <c>null</c>.</exception>
    public static App<A> LiftIO(IO<A> io)
    {
        ArgumentNullException.ThrowIfNull(io);
        return new App<A>(ReaderT<AppEnvironment, IO>.liftIO(io));
    }

    /// <summary>
    /// Runs the application computation with the provided <see cref="AppEnvironment"/>
    /// and returns the resulting <see cref="IO{A}"/> effect.
    /// </summary>
    /// <param name="env">The application environment used by the ReaderT.</param>
    /// <returns>An <see cref="IO{A}"/> representing the executed computation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="env"/> is <c>null</c>.</exception>
    public IO<A> Run(AppEnvironment env)
    {
        ArgumentNullException.ThrowIfNull(env);
        return RunApp.Run(env).As();
    }
}
