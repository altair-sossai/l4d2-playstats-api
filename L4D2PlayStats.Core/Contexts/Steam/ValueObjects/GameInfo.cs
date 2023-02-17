using System.Text.Json.Serialization;

namespace L4D2PlayStats.Core.Contexts.Steam.ValueObjects;

public class GameInfo
{
    [JsonPropertyName("appid")]
    public int? AppId { get; set; }

    [JsonPropertyName("playtime_forever")]
    public int? PlayTimeForever { get; set; }
}