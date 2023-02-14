using System.Text.Json.Serialization;

namespace L4D2PlayStats.Core.Contexts.Steam.ValueObjects;

public class PlayerInfo
{
	[JsonPropertyName("steamid")]
	public string? SteamId { get; set; }

	[JsonPropertyName("personaname")]
	public string? PersonaName { get; set; }

	[JsonPropertyName("profileurl")]
	public string? ProfileUrl { get; set; }

	[JsonPropertyName("avatarfull")]
	public string? AvatarFull { get; set; }
}