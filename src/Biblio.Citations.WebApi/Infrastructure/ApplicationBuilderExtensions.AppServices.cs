
using Akka.Actor;
using Akka.Hosting;
using Biblio.Citations.Services;
using Biblio.Common.Akka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Biblio.Citations.Infrastructure;

public static partial class ApplicationBuilderExtensions
{
    /// <summary>
    /// Registers application services.
    /// </summary>
    public static IHostApplicationBuilder WithAppServices(this IHostApplicationBuilder appBuilder)
    {
        appBuilder.Services.AddTransient<IActorProvider>(sp =>
        {
            var actorRegistry = sp.GetRequiredService<IReadOnlyActorRegistry>();
            return new ActorRegistryProvider(actorRegistry);
        });

        appBuilder.Services.AddTransient(sp =>
        {
            var actorProvider = sp.GetRequiredService<IActorProvider>();
            var actorSystem = sp.GetRequiredService<ActorSystem>();
            return new AppEnvironment(actorProvider, actorSystem);
        });

        return appBuilder;
    }
}
