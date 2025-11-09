using Biblio.Citations.Domain.Tests.Common;

namespace Biblio.Citations.Domain.Tests;

/// <summary>
/// Represents the mutable state used in test runs. Currently it holds the <see cref="TestActorProvider"/>
/// which maps actor key types to their <see cref="Akka.Actor.IActorRef"/> instances.
/// </summary>
/// <param name="ActorProvider">The provider used to resolve actor references during tests.</param>
public sealed record TestState(TestActorProvider ActorProvider);