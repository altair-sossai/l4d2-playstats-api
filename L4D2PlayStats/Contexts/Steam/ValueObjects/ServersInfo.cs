using System.Text.Json.Serialization;

namespace L4D2PlayStats.Contexts.Steam.ValueObjects;

public class ServersInfo
{
    [JsonPropertyName("servers")]
    public List<ServerInfo?>? Servers { get; set; }
}