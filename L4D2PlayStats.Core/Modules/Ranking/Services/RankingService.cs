using L4D2PlayStats.Core.Modules.Matches.Services;
using L4D2PlayStats.Core.Modules.Ranking.Configs;
using L4D2PlayStats.Core.Modules.Ranking.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Core.Modules.Ranking.Services;

public class RankingService(IMemoryCache memoryCache, IMatchService matchService, IExperienceConfig config) : IRankingService
{
    public Task<List<Player>> RankingAsync(string serverId)
    {
        return memoryCache.GetOrCreateAsync($"ranking_{serverId}".ToLower(), async factory =>
        {
            factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

            var matches = await matchService.GetMatchesAsync(serverId);
            var players = matches.Ranking(config).ToList();

            return players;
        })!;
    }
}