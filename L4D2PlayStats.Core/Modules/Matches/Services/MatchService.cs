using L4D2PlayStats.Core.Modules.Campaigns.Repositories;
using L4D2PlayStats.Core.Modules.Statistics.Extensions;
using L4D2PlayStats.Core.Modules.Statistics.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Core.Modules.Matches.Services;

public class MatchService : IMatchService
{
    private readonly ICampaignRepository _campaignRepository;
    private readonly IMemoryCache _memoryCache;
    private readonly IStatisticsRepository _statisticsRepository;

    public MatchService(IMemoryCache memoryCache,
        IStatisticsRepository statisticsRepository,
        ICampaignRepository campaignRepository)
    {
        _memoryCache = memoryCache;
        _statisticsRepository = statisticsRepository;
        _campaignRepository = campaignRepository;
    }

    public async Task<Match?> LastMatchAsync(string serverId)
    {
        var match = await _memoryCache.GetOrCreateAsync($"ranking_last_match_{serverId}".ToLower(), async factory =>
        {
            factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

            var matches = await GetMatchesAsync(serverId);
            var match = matches.FirstOrDefault();

            return match;
        });

        return match;
    }

    public async Task<List<Match>> GetMatchesAsync(string serverId)
    {
        var matches = await _memoryCache.GetOrCreateAsync($"matches_{serverId}".ToLower(), async factory =>
        {
            factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

            var campaigns = _campaignRepository.GetCampaigns();
            var matches = await _statisticsRepository.GetStatisticsAsync(serverId).ToMatchesAsync(campaigns);

            return matches;
        });

        return matches;
    }

    public async Task<List<Match>> GetMatchesBetweenAsync(string serverId, string start, string end)
    {
        var campaigns = _campaignRepository.GetCampaigns();
        var matches = await _statisticsRepository.GetStatisticsBetweenAsync(serverId, start, end).ToMatchesAsync(campaigns);

        return matches;
    }
}