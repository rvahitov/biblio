using Akka.Cluster.Hosting;
using Akka.Remote.Hosting;
using Microsoft.Extensions.Configuration;

namespace Biblio.Citations.Infrastructure.Options;


internal sealed class AkkaOptions
{
    public const string SectionName = "Akka";
    public string ActorSystemName { get; set; } = "citations";

    public bool UseClustering { get; set; } = true;

    public bool LogConfigOnStart { get; set; } = false;

    public RemoteOptions RemoteOptions { get; set; } = new();

    public ClusterOptions ClusterOptions { get; set; } = new();

    public ShardOptions ShardOptions { get; set; } = new();

    public static AkkaOptions LoadFromConfiguration(IConfiguration configuration)
    {
        var section = configuration.GetRequiredSection(SectionName);
        var result = new AkkaOptions();
        section.Bind(result);
        return result;
    }
}