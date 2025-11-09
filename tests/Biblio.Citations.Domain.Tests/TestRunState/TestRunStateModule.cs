using Akka.Actor;
using LanguageExt;
using LanguageExt.Traits;

namespace Biblio.Citations.Domain.Tests;

/// <summary>
/// Module helpers for working with <see cref="TestRunState"/> computations.
/// Contains convenience functions used in tests to manipulate the test state.
/// </summary>
public partial class TestRunState
{
    /// <summary>
    /// Register an <see cref="IActorRef"/> for the given actor key type <typeparamref name="A"/> in the current test state.
    /// This returns a <see cref="TestRunState{Unit}"/> computation that updates the stored <see cref="TestState"/>.
    /// </summary>
    /// <typeparam name="A">The actor key type for which the reference is registered.</typeparam>
    /// <param name="actorRef">The actor reference to register.</param>
    /// <returns>A <see cref="TestRunState{Unit}"/> that performs the registration as a state modification.</returns>
    public static TestRunState<Unit> AddActorRef<A>(IActorRef actorRef) =>
        Stateful.modify<TestRunState, TestState>(
            state => new TestState(state.ActorProvider.AddActor<A>(actorRef))
        ).As();
}
