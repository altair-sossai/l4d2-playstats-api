using System.Text.Json.Serialization;

namespace L4D2PlayStats.Core.Contexts.Steam.ValueObjects;

public class GamesInfo
{
    [JsonPropertyName("games")]
    public List<GameInfo?>? Games { get; set; }
}