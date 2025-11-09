using System;
using Biblio.Common.Akka;
using LanguageExt;
using LanguageExt.Traits;

namespace Biblio.Citations.Domain.Tests;

/// <summary>
/// Kind-level wiring for the <see cref="TestRunState"/> monad transformer stack used in tests.
/// Implements various typeclass-derived interfaces required by the LanguageExt-style abstractions used in the project.
/// </summary>
public abstract partial class TestRunState :
    Deriving<TestRunState, StateT<TestState, IO>>,
    Deriving.MonadT<TestRunState, StateT<TestState, IO>, IO>,
    Deriving.Stateful<TestRunState, StateT<TestState, IO>, TestState>,
    Deriving.Choice<TestRunState, StateT<TestState, IO>>,
    MonadIO<TestRunState>,
    Has<TestRunState, IActorProvider>
{
    /// <summary>
    /// Ask implementation for the <see cref="Has{TKind, T}"/> capability to retrieve the <see cref="IActorProvider"/> from the state.
    /// </summary>
    static K<TestRunState, IActorProvider> Has<TestRunState, IActorProvider>.Ask =>
        Stateful.gets<TestRunState, TestState, IActorProvider>(s => s.ActorProvider);

    /// <summary>
    /// Co-natural transformation that lifts a <see cref="K{StateT, A}"/> into the <see cref="TestRunState{A}"/> kind.
    /// </summary>
    /// <typeparam name="A">The value type of the computation.</typeparam>
    /// <param name="fa">The computation to lift.</param>
    /// <returns>The lifted computation as a <see cref="K{TestRunState, A}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="fa"/> is <c>null</c>.</exception>
    static K<TestRunState, A> CoNatural<TestRunState, StateT<TestState, IO>>.CoTransform<A>(K<StateT<TestState, IO>, A> fa)
    {
        ArgumentNullException.ThrowIfNull(fa);
        return new TestRunState<A>(fa);
    }

    /// <summary>
    /// Lift an <see cref="IO{A}"/> into the <see cref="TestRunState{A}"/> context.
    /// </summary>
    /// <typeparam name="A">The result type of the IO action.</typeparam>
    /// <param name="ma">The IO action to lift.</param>
    /// <returns>The lifted computation as a <see cref="K{TestRunState, A}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="ma"/> is <c>null</c>.</exception>
    static K<TestRunState, A> MonadIO<TestRunState>.LiftIO<A>(IO<A> ma)
    {
        ArgumentNullException.ThrowIfNull(ma);
        return new TestRunState<A>(StateT<TestState, IO, A>.LiftIO(ma));
    }

    /// <summary>
    /// Natural transformation from <see cref="TestRunState{A}"/> to the underlying <see cref="StateT{TestState, IO, A}"/> representation.
    /// </summary>
    /// <typeparam name="A">The value type of the computation.</typeparam>
    /// <param name="fa">The kind instance to transform.</param>
    /// <returns>The underlying <see cref="K{StateT, A}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="fa"/> is <c>null</c>.</exception>
    static K<StateT<TestState, IO>, A> Natural<TestRunState, StateT<TestState, IO>>.Transform<A>(K<TestRunState, A> fa)
    {
        ArgumentNullException.ThrowIfNull(fa);
        return fa.As().State;
    }
}
