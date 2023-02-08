using System.Text.Json.Serialization;

namespace L4D2PlayStats.Contexts.Steam.ValueObjects;

public class ResolveVanityUrl
{
    [JsonPropertyName("steamid")]
    public string? SteamId { get; set; }

    [JsonPropertyName("success")]
    public int? Success { get; set; }
}