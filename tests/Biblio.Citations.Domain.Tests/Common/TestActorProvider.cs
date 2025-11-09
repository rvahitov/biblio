using System;
using System.Collections.Generic;
using Akka.Actor;
using Biblio.Common.Akka;
using LanguageExt;

namespace Biblio.Citations.Domain.Tests.Common;

public sealed class TestActorProvider(HashMap<string, IActorRef> actors) : IActorProvider
{
    public static TestActorProvider Create() => new([]);

    public TestActorProvider AddActor<A>(IActorRef actorRef)
    {
        var typeQualifiedName = typeof(A).AssemblyQualifiedName
            ?? throw new InvalidOperationException($"AssemblyQualifiedName is null for type {typeof(A)}");

        if (actors.ContainsKey(typeQualifiedName))
        {
            return this;
        }
        var updatedMap = actors.Add(typeQualifiedName, actorRef);
        return new TestActorProvider(updatedMap);
    }

    IO<IActorRef> IActorProvider.GetActor<A>()
    {
        var actorMap = actors;
        return IO.lift(() =>
        {
            var typeQualifiedName = typeof(A).AssemblyQualifiedName
                ?? throw new InvalidOperationException($"AssemblyQualifiedName is null for type {typeof(A)}");

            var actor = actorMap.Find(typeQualifiedName)
                .IfNone(() => throw new KeyNotFoundException($"No actor registered for type '{typeQualifiedName}' in TestActorProvider"));
            return actor;
        });
    }
}
