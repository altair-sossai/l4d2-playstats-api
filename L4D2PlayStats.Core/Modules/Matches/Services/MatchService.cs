﻿using L4D2PlayStats.Core.Modules.Campaigns.Repositories;
using L4D2PlayStats.Core.Modules.Statistics.Extensions;
using L4D2PlayStats.Core.Modules.Statistics.Repositories;

namespace L4D2PlayStats.Core.Modules.Matches.Services;

public class MatchService : IMatchService
{
    private readonly ICampaignRepository _campaignRepository;
    private readonly IStatisticsRepository _statisticsRepository;

    public MatchService(IStatisticsRepository statisticsRepository,
        ICampaignRepository campaignRepository)
    {
        _statisticsRepository = statisticsRepository;
        _campaignRepository = campaignRepository;
    }

    public async Task<Match?> LastMatchAsync(string server)
    {
        var matches = await GetMatchesAsync(server);
        var match = matches.FirstOrDefault();

        return match;
    }

    public async Task<List<Match>> GetMatchesAsync(string server)
    {
        var campaigns = _campaignRepository.GetCampaigns();
        var matches = await _statisticsRepository.GetStatisticsAsync(server).ToMatchesAsync(campaigns);

        return matches;
    }

    public async Task<List<Match>> GetMatchesBetweenAsync(string server, string start, string end)
    {
        var campaigns = _campaignRepository.GetCampaigns();
        var matches = await _statisticsRepository.GetStatisticsBetweenAsync(server, start, end).ToMatchesAsync(campaigns);

        return matches;
    }
}