using System;
using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

namespace Biblio.Aspire.Resources.KurrentDb;

/// <summary>
/// Provides extension methods to register and configure a KurrentDb (Event Store) resource
/// in an <see cref="IDistributedApplicationBuilder"/> or to further configure an
/// <see cref="IResourceBuilder{KurrentDbResource}"/> instance.
/// </summary>
public static class KurrentDbExtensions
{
    private const int TargetPort = 2113;

    /// <summary>
    /// Adds a KurrentDb resource to the distributed application builder with a primary HTTP endpoint.
    /// </summary>
    /// <param name="builder">The distributed application builder to register the resource with.</param>
    /// <param name="name">Logical name for the resource (used for endpoint naming and discovery).</param>
    /// <param name="port">Optional host port to map to the service's internal port. If null, a dynamic host port will be selected.</param>
    /// <returns>An <see cref="IResourceBuilder{KurrentDbResource}"/> for further fluent configuration.</returns>
    public static IResourceBuilder<KurrentDbResource> AddKurrentDb(
        this IDistributedApplicationBuilder builder,
        [ResourceName] string name,
        int? port = null
    )
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        var resource = new KurrentDbResource(name);
        var kurrentDb = builder.AddResource(resource);
        kurrentDb.WithImage(KurrentDbContainerImages.Image, KurrentDbContainerImages.Tag);
        kurrentDb.WithHttpEndpoint(name: KurrentDbResource.PrimaryEndpointName, targetPort: TargetPort, port: port);
        return kurrentDb;
    }

    /// <summary>
    /// Configures the KurrentDb resource to run all projections and start the standard projections.
    /// </summary>
    /// <param name="kurrentDb">The resource builder to configure.</param>
    /// <returns>The same <see cref="IResourceBuilder{KurrentDbResource}"/> for chaining.</returns>
    /// <remarks>This enables all projections, which can be resource-intensive. Use cautiously in production environments.</remarks>
    public static IResourceBuilder<KurrentDbResource> RunAllProjections(
        this IResourceBuilder<KurrentDbResource> kurrentDb
    )
    {
        ArgumentNullException.ThrowIfNull(kurrentDb);
        kurrentDb
                .WithEnvironment("KURRENTDB_RUN_PROJECTIONS", "ALL")
                .WithEnvironment("KURRENTDB_START_STANDARD_PROJECTIONS", "true");
        return kurrentDb;
    }

    /// <summary>
    /// Enables insecure mode for the KurrentDb resource. This is intended for local development only.
    /// </summary>
    /// <param name="kurrentDb">The resource builder to configure.</param>
    /// <returns>The same <see cref="IResourceBuilder{KurrentDbResource}"/> for chaining.</returns>
    /// <remarks>Insecure mode disables TLS and other security features. Do not use in production environments.</remarks>
    public static IResourceBuilder<KurrentDbResource> EnableInsecureMode(
            this IResourceBuilder<KurrentDbResource> kurrentDb
    )
    {
        ArgumentNullException.ThrowIfNull(kurrentDb);
        kurrentDb.WithEnvironment("KURRENTDB_INSECURE", "true");
        return kurrentDb;
    }

    /// <summary>
    /// Enables AtomPub (Atom Publishing Protocol) over HTTP for the KurrentDb resource.
    /// </summary>
    /// <param name="kurrentDb">The resource builder to configure.</param>
    /// <returns>The same <see cref="IResourceBuilder{KurrentDbResource}"/> for chaining.</returns>
    /// <remarks>Enabling AtomPub over HTTP allows clients to interact with the KurrentDb instance using the AtomPub protocol.</remarks>
    public static IResourceBuilder<KurrentDbResource> EnableAtomPubOverHttp(
            this IResourceBuilder<KurrentDbResource> kurrentDb
    )
    {
        ArgumentNullException.ThrowIfNull(kurrentDb);
        kurrentDb.WithEnvironment("KURRENTDB_ENABLE_ATOM_PUB_OVER_HTTP", "true");
        return kurrentDb;
    }
}
