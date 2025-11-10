using Aspire.Hosting.ApplicationModel;

namespace Biblio.Aspire.Resources;

/// <summary>
/// Represents a KurrentDb container resource used by the distributed application host.
/// Provides a reference to the primary endpoint and a connection-string expression
/// that clients can use to connect to the running Event Store / KurrentDB instance.
/// </summary>
public sealed class KurrentDbResource :
    ContainerResource,
    IResourceWithConnectionString
{
    /// <summary>
    /// Name of the primary endpoint exposed by the KurrentDb resource.
    /// </summary>
    public const string PrimaryEndpointName = "primary";

    private const string ConnectionStringScheme = "esdb";

    /// <summary>
    /// Initializes a new instance of the <see cref="KurrentDbResource"/> class.
    /// </summary>
    /// <param name="name">Logical name for the resource. This name is used in endpoint naming and discovery.</param>
    public KurrentDbResource([ResourceName] string name) : base(name)
    {
        PrimaryEndpoint = new EndpointReference(this, PrimaryEndpointName);
    }

    /// <summary>
    /// Reference to the primary HTTP endpoint exposed by the KurrentDb container.
    /// Use this reference to resolve host/port information for runtime wiring.
    /// </summary>
    public EndpointReference PrimaryEndpoint { get; }

    /// <summary>
    /// Expression resolving to the connection string for clients to connect to this KurrentDb instance.
    /// The expression uses the endpoint's host and port and explicitly disables TLS (tls=false) to
    /// simplify local development scenarios.
    /// </summary>
    /// <remarks>
    /// Example result: <c>esdb://{host}:{port}?tls=false</c>
    /// For production deployments consider enabling TLS and adjusting query parameters accordingly.
    /// </remarks>
    public ReferenceExpression ConnectionStringExpression => ReferenceExpression.Create(
        $"{ConnectionStringScheme}://{PrimaryEndpoint.Property(EndpointProperty.HostAndPort)}?tls=false"
    );
}
