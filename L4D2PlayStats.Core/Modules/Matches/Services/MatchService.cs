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

    public async Task<List<Match>> GetMatchesAsync(string serverId, DateTime? reference = null, bool competitiveOnly = true)
    {
        var server = serverService.GetServer(serverId);
        if (server == null)
            return [];

        var campaigns = campaignRepository.GetCampaigns();
        var statistics = statisticsRepository.GetStatisticsAsync(serverId, reference);

        if (competitiveOnly)
            statistics = statistics.Where(s => server.RankingConfiguration(s.ConfigurationName));

        var matches = await statistics.ToMatchesAsync(campaigns, competitiveOnly);

        return matches;
    }

    public async Task<List<Match>> GetMatchesAsync(string serverId, DateTime start, DateTime end, bool competitiveOnly = true)
    {
        var server = serverService.GetServer(serverId);
        if (server == null)
            return [];

        var campaigns = campaignRepository.GetCampaigns();
        var statistics = statisticsRepository.GetStatisticsAsync(serverId, start, end);

        if (competitiveOnly)
            statistics = statistics.Where(s => server.RankingConfiguration(s.ConfigurationName));

        var matches = await statistics.ToMatchesAsync(campaigns, competitiveOnly);

        return matches;
    }

    public async Task<List<Match>> GetMatchesAsync(string serverId, string start, string end, bool competitiveOnly = true)
    {
        var server = serverService.GetServer(serverId);
        if (server == null)
            return [];

        var campaigns = campaignRepository.GetCampaigns();
        var statistics = statisticsRepository.GetStatisticsBetweenAsync(serverId, start, end);

        if (competitiveOnly)
            statistics = statistics.Where(s => server.RankingConfiguration(s.ConfigurationName));

        var matches = await statistics.ToMatchesAsync(campaigns, competitiveOnly);

        return matches;
    }

    public async Task<List<Match>> GetAllMatchesAsync(string serverId, bool competitiveOnly = true)
    {
        var server = serverService.GetServer(serverId);
        if (server == null)
            return [];

        var campaigns = campaignRepository.GetCampaigns();
        var statistics = statisticsRepository.GetAllStatisticsAsync(serverId);

        if (competitiveOnly)
            statistics = statistics.Where(s => server.RankingConfiguration(s.ConfigurationName));

        var matches = await statistics.ToMatchesAsync(campaigns, competitiveOnly);

        return matches;
    }
}