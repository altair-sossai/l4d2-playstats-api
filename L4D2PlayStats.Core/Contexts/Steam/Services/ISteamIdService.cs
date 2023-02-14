namespace L4D2PlayStats.Core.Contexts.Steam.Services;

public interface ISteamIdService
{
	Task<long?> ResolveSteamIdAsync(string? customUrl);
}