using L4D2PlayStats.Core.Modules.Matches.Services;
using L4D2PlayStats.Core.Modules.PlayerStatistics.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Core.Modules.PlayerStatistics.Services;

public class PlayerStatisticsService(
    IMemoryCache memoryCache,
    IMatchService matchService)
    : IPlayerStatisticsService
{
    public Task<List<Player>> PlayerStatisticsAsync(string serverId)
    {
        return memoryCache.GetOrCreateAsync($"player_statistics_{serverId}".ToLower(), async factory =>
        {
            factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

            var matches = await matchService.GetMatchesAsync(serverId);
            var players = matches.PlayerStatistics().ToList();

            return players;
        })!;
    }
}