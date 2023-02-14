using L4D2PlayStats.Core.Modules.Campaigns.Extensions;
using L4D2PlayStats.Core.Modules.Campaigns.Repositories;
using L4D2PlayStats.Core.Modules.Matches.Results;
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

	public async Task<List<MatchResult>> GetMatchesAsync(string server, int statisticsCount)
	{
		var campaigns = _campaignRepository.GetCampaigns();
		var maps = campaigns.Maps();
		var matches = new List<MatchResult>();

		MatchResult? match = null;
		string? lastMap = null;

		await foreach (var statistics in _statisticsRepository.GetStatisticsAsync(server).Take(statisticsCount))
		{
			var statistic = statistics.Statistic;
			if (statistic == null)
				continue;

			var gameRound = statistic.GameRound;
			var mapName = gameRound?.MapName;
			var scoring = statistic.Scoring;
			var teamA = scoring?.TeamA;
			var teamB = scoring?.TeamB;
			var halfA = statistic.Halves.FirstOrDefault(half => half.RoundHalf?.Team == 'A');
			var halfB = statistic.Halves.FirstOrDefault(half => half.RoundHalf?.Team == 'B');

			if (gameRound == null || mapName == null || teamA == null || teamB == null || halfA == null || halfB == null || !maps.ContainsKey(mapName))
				continue;

			var campaign = maps[mapName];

			if (match == null || string.IsNullOrEmpty(lastMap) || !campaign.SequentialMaps(lastMap, mapName))
			{
				match = new MatchResult(gameRound, campaign);
				matches.Add(match);
			}

			var playersA = statistic.PlayerNames.Where(playerName => halfA.Players.Any(p => p.CommunityId == playerName.CommunityId)).ToList();
			var playersB = statistic.PlayerNames.Where(playerName => halfB.Players.Any(p => p.CommunityId == playerName.CommunityId)).ToList();

			match.Update(statistics.RowKey, teamA, playersA, teamB, playersB);

			lastMap = mapName;
		}

		return matches;
	}
}