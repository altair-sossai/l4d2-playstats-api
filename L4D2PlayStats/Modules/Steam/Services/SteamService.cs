using L4D2PlayStats.Contexts.Steam;
using L4D2PlayStats.Contexts.Steam.Services;
using L4D2PlayStats.Contexts.Steam.ValueObjects;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Modules.Steam.Services;

public class SteamService : ISteamService
{
    private readonly IMemoryCache _memoryCache;
    private readonly ISteamContext _steamContext;
    private readonly ISteamPlayerService _steamPlayerService;
    private readonly ISteamUserService _steamUserService;

    public SteamService(IMemoryCache memoryCache,
        ISteamContext steamContext,
        ISteamPlayerService steamPlayerService,
        ISteamUserService steamUserService)
    {
        _memoryCache = memoryCache;
        _steamContext = steamContext;
        _steamPlayerService = steamPlayerService;
        _steamUserService = steamUserService;
    }

    public Task<GamesInfo?> GetOwnedGamesAsync(long communityId)
    {
        return _memoryCache.GetOrCreateAsync($"GetOwnedGames_{communityId}", async factory =>
        {
            factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

            var responseData = await _steamPlayerService.GetOwnedGamesAsync(_steamContext.SteamApiKey, communityId.ToString());

            return responseData.Response;
        });
    }

    public Task<PlayersInfo?> GetPlayerSummariesAsync(long communityId)
    {
        return _memoryCache.GetOrCreateAsync($"GetPlayerSummaries_{communityId}", async factory =>
        {
            factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

            var responseData = await _steamUserService.GetPlayerSummariesAsync(_steamContext.SteamApiKey, communityId.ToString());

            return responseData.Response;
        });
    }
}