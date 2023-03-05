using L4D2PlayStats.Core.Modules.Matches.Extensions;
using L4D2PlayStats.Core.Modules.Matches.Services;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Core.Modules.Ranking.Services;

public class RankingService : IRankingService
{
    private readonly IMatchService _matchService;
    private readonly IMemoryCache _memoryCache;

    public RankingService(IMemoryCache memoryCache,
        IMatchService matchService)
    {
        _memoryCache = memoryCache;
        _matchService = matchService;
    }

    public async Task<List<Player>> RankingAsync(string server)
    {
        return await _memoryCache.GetOrCreateAsync($"ranking_{server}".ToLower(), async factory =>
        {
            factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

            var matches = await _matchService.GetMatchesAsync(server);
            var players = matches.Ranking().ToList();

            return players;
        });
    }
}