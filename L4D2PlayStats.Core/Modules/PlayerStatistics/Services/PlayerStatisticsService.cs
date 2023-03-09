using L4D2PlayStats.Core.Modules.Matches.Services;
using L4D2PlayStats.Core.Modules.PlayerStatistics.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Core.Modules.PlayerStatistics.Services;

public class PlayerStatisticsService : IPlayerStatisticsService
{
    private readonly IMatchService _matchService;
    private readonly IMemoryCache _memoryCache;

    public PlayerStatisticsService(IMemoryCache memoryCache,
        IMatchService matchService)
    {
        _memoryCache = memoryCache;
        _matchService = matchService;
    }

    public async Task<List<Player>> PlayerStatisticsAsync(string serverId)
    {
        return await _memoryCache.GetOrCreateAsync($"player_statistics_{serverId}".ToLower(), async factory =>
        {
            factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

            var matches = await _matchService.GetMatchesAsync(serverId);
            var players = matches.PlayerStatistics().ToList();

            return players;
        });
    }
}