using Akka.Actor;
using Biblio.Common.Akka;

namespace Biblio.Citations.Services;

public sealed record AppEnvironment(
    IActorProvider ActorProvider,
    ActorSystem ActorSystem
);
