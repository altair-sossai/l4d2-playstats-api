using L4D2PlayStats.Core.Modules.Campaigns;
using L4D2PlayStats.Core.Modules.Campaigns.Extensions;
using L4D2PlayStats.Core.Modules.Matches;

namespace L4D2PlayStats.Core.Modules.Statistics.Extensions;

public static class StatisticsExtensions
{
    public static async Task<List<Match>> ToMatchesAsync(this IAsyncEnumerable<Statistics> statistics, IEnumerable<Campaign> campaigns)
    {
        var maps = campaigns.Maps();
        var matches = new List<Match>();

        Match? match = null;
        string? lastMap = null;

        await foreach (var statistic in statistics)
        {
            var stats = statistic.Statistic;
            if (stats == null)
                continue;

            var gameRound = stats.GameRound;
            var mapName = gameRound?.MapName;
            var scoring = stats.Scoring;
            var teamA = scoring?.TeamA;
            var teamB = scoring?.TeamB;
            var halfA = stats.Halves.FirstOrDefault(half => half.RoundHalf?.Team == 'A');
            var halfB = stats.Halves.FirstOrDefault(half => half.RoundHalf?.Team == 'B');

            if (gameRound == null || mapName == null || teamA == null || teamB == null || halfA == null || halfB == null || !maps.ContainsKey(mapName))
                continue;

            var campaign = maps[mapName];

            if (match == null || string.IsNullOrEmpty(lastMap) || !campaign.SequentialMaps(mapName, lastMap))
            {
                var playersA = stats.PlayerNames.Where(playerName => halfA.Players.Any(p => p.CommunityId == playerName.CommunityId)).ToList();
                var playersB = stats.PlayerNames.Where(playerName => halfB.Players.Any(p => p.CommunityId == playerName.CommunityId)).ToList();

                match = new Match(campaign, teamA, playersA, teamB, playersB)
                {
                    MatchEnd = stats.MapEnd
                };

                matches.Add(match);
            }

            match.MatchStart = gameRound.When;
            match.Add(statistic);

            lastMap = mapName;
        }

        return matches
            .Where(w => w.Maps.Count >= 4)
            .ToList();
    }
}