using System;
using Akka.Actor;
using Akka.Cluster.Hosting;
using Akka.Event;
using Akka.Hosting;
using Akka.Logger.Serilog;
using Akka.Persistence.EventStore.Hosting;
using Akka.Remote.Hosting;
using Biblio.Citations.Domain.BookDomain.Actors;
using Biblio.Citations.Domain.CitationDomain.Actors;
using Biblio.Citations.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Biblio.Citations.Infrastructure;

public static partial class ApplicationBuilderExtensions
{
    private const string EventStoreConnectionStringName = "KurrentDb";
    /// <summary>
    /// Registers and configures Akka.NET related services on the provided host application builder.
    /// Loads <c>AkkaOptions</c> from the application's configuration, adds the loaded options as a singleton
    /// service to the dependency injection container, and applies the Akka hosting configuration.
    /// </summary>
    /// <param name="appBuilder">The <see cref="IHostApplicationBuilder"/> to configure.</param>
    /// <returns>The same <see cref="IHostApplicationBuilder"/> instance to allow fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="appBuilder"/> is <c>null</c>.</exception>
    /// <remarks>
    /// After calling this method, components can resolve the configured <c>AkkaOptions</c> from DI,
    /// and the Akka hosting extensions will have been applied to the application's services.
    /// </remarks>
    public static IHostApplicationBuilder WithAkka(this IHostApplicationBuilder appBuilder)
    {
        ArgumentNullException.ThrowIfNull(appBuilder);
        var akkaOptions = AkkaOptions.LoadFromConfiguration(appBuilder.Configuration);
        appBuilder.Services.AddSingleton(akkaOptions);
        appBuilder.Services.ConfigureAkkaHosting(akkaOptions);
        return appBuilder;
    }

    /// <summary>
    /// Configures Akka.NET hosting services on the provided <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="akkaOptions">The Akka options used to configure the actor system.</param>
    /// <param name="additionalConfig">An optional action to apply additional configuration to the Akka configuration builder.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="akkaOptions"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="akkaOptions"/> contains an invalid <c>ActorSystemName</c>.</exception>
    private static void ConfigureAkkaHosting(
        this IServiceCollection services,
        AkkaOptions akkaOptions,
        Action<AkkaConfigurationBuilder, IServiceProvider>? additionalConfig = null
    )
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(akkaOptions);
        ArgumentException.ThrowIfNullOrWhiteSpace(akkaOptions.ActorSystemName);
        var adConf = additionalConfig ?? ((_, _) => { });
        services.AddAkka(akkaOptions.ActorSystemName, (builder, sp) =>
        {
            builder.ConfigureLoggers(loggerBuilder =>
                {
                    loggerBuilder.LogConfigOnStart = akkaOptions.LogConfigOnStart;
                    loggerBuilder.LogLevel = LogLevel.InfoLevel;
                    loggerBuilder.ClearLoggers();
                    loggerBuilder.AddLogger<SerilogLogger>();
                })
                .WithRemoting(akkaOptions.RemoteOptions)
                .WithClustering(akkaOptions.ClusterOptions);
            var config = sp.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString(EventStoreConnectionStringName);
            ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
            builder.WithEventStorePersistence(connectionString: connectionString);
            builder.AddCitationActors(akkaOptions);
            adConf(builder, sp);
        });
    }

    /// <summary>
    /// Adds citation-related actors to the Akka configuration builder.
    /// </summary>
    /// <param name="akkaBuilder">The Akka configuration builder to which the actors will be added.</param>
    /// <param name="akkaOptions">The Akka options used to configure shard regions.</param>
    private static void AddCitationActors(this AkkaConfigurationBuilder akkaBuilder, AkkaOptions akkaOptions)
    {
        akkaBuilder.WithSingleton<BookCollectionActor>("books", Props.Create(() => new BookCollectionActor("books")));

        akkaBuilder.WithShardRegion<CitationActor>("citation", CitationActor.CreateProps,
            CitationActor.CreateMessageExtractor(), akkaOptions.ShardOptions);
    }
}
