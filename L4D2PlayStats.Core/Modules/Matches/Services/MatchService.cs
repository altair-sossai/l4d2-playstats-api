using L4D2PlayStats.Core.Modules.Campaigns.Repositories;
using L4D2PlayStats.Core.Modules.Server.Services;
using L4D2PlayStats.Core.Modules.Statistics.Extensions;
using L4D2PlayStats.Core.Modules.Statistics.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Core.Modules.Matches.Services;

public class MatchService(
    IMemoryCache memoryCache,
    IServerService serverService,
    IStatisticsRepository statisticsRepository,
    ICampaignRepository campaignRepository)
    : IMatchService
{
    public async Task<Match?> LastMatchAsync(string serverId)
    {
        var match = await memoryCache.GetOrCreateAsync($"ranking_last_match_{serverId}".ToLower(), async factory =>
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
        var matches = await memoryCache.GetOrCreateAsync($"matches_{serverId}".ToLower(), async factory =>
        {
            factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

            var server = serverService.GetServer(serverId);
            if (server == null)
                return [];

            var campaigns = campaignRepository.GetCampaigns();
            var matches = await statisticsRepository
                .GetStatisticsAsync(serverId)
                .Where(statistics => server.RankingConfiguration(statistics.ConfigurationName))
                .ToMatchesAsync(campaigns);

            return matches;
        });

        return matches!;
    }

    public async Task<List<Match>> GetMatchesBetweenAsync(string serverId, string start, string end)
    {
        var server = serverService.GetServer(serverId);
        if (server == null)
            return [];

        var campaigns = campaignRepository.GetCampaigns();
        var matches = await statisticsRepository
            .GetStatisticsBetweenAsync(serverId, start, end)
            .Where(statistics => server.RankingConfiguration(statistics.ConfigurationName))
            .ToMatchesAsync(campaigns);

        return matches;
    }
}