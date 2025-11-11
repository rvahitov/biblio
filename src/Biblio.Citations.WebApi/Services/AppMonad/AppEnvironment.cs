using Akka.Actor;
using Biblio.Common.Akka;

namespace Biblio.Citations.Services.AppMonad;

public sealed record AppEnvironment(
    IActorProvider ActorProvider,
    ActorSystem ActorSystem
);
