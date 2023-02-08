namespace L4D2PlayStats.Contexts.Steam.Services;

public interface ISteamIdService
{
    Task<long?> ResolveSteamIdAsync(string? customUrl);
}