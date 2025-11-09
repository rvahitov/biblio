using Biblio.Citations.Domain.Tests.Common;
using LanguageExt;
using LanguageExt.Traits;

namespace Biblio.Citations.Domain.Tests;

/// <summary>
/// Represents a test run computation that carries a <see cref="TestState"/> and produces a value of type <typeparamref name="A"/>.
/// This type wraps a <see cref="K{StateT, A}"/> where the underlying monad is a <see cref="StateT{TestState, IO}"/>.
/// </summary>
/// <typeparam name="A">The result value type produced by the computation.</typeparam>
/// <param name="state">The underlying stateful computation.</param>
public sealed class TestRunState<A>(K<StateT<TestState, IO>, A> state) : K<TestRunState, A>
{
    /// <summary>
    /// Gets the underlying state transformer computation for this test run state.
    /// </summary>
    public K<StateT<TestState, IO>, A> State { get; } = state;

    /// <summary>
    /// Execute the stateful computation, producing both the value and the final <see cref="TestState"/>.
    /// </summary>
    /// <returns>An <see cref="IO{T}"/> that yields a tuple of the result value and the final state.</returns>
    public IO<(A Value, TestState State)> Run() => State.Run(new TestState(TestActorProvider.Create())).As();

    /// <summary>
    /// Evaluate the computation and return only the resulting value.
    /// </summary>
    /// <returns>An <see cref="IO{T}"/> that yields the result value.</returns>
    public IO<A> Eval() => Run().Map(t => t.Value);

    /// <summary>
    /// Execute the computation and return only the final <see cref="TestState"/>.
    /// </summary>
    /// <returns>An <see cref="IO{T}"/> that yields the final <see cref="TestState"/>.</returns>
    public IO<TestState> Exec() => Run().Map(t => t.State);
}
