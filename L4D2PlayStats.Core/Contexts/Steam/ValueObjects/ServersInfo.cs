using System.Text.Json.Serialization;

namespace L4D2PlayStats.Core.Contexts.Steam.ValueObjects;

public class ServersInfo
{
	[JsonPropertyName("servers")]
	public List<ServerInfo?>? Servers { get; set; }
}