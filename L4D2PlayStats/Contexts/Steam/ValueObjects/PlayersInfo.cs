using System.Text.Json.Serialization;

namespace L4D2PlayStats.Contexts.Steam.ValueObjects;

public class PlayersInfo
{
    [JsonPropertyName("players")]
    public List<PlayerInfo?>? Players { get; set; }
}