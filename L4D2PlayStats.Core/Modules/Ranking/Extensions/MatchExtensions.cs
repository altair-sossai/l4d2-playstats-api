using L4D2PlayStats.Core.Modules.Matches;
using L4D2PlayStats.Core.Modules.Matches.Structures;
using L4D2PlayStats.Core.Modules.Ranking.Extensions.L4D2PlayStats;
using L4D2PlayStats.Core.Modules.Statistics.Extensions.L4D2PlayStats;

namespace L4D2PlayStats.Core.Modules.Ranking.Extensions;

public static class MatchExtensions
{
    public static IEnumerable<Player> Ranking(this Match match)
    {
        var matches = new[] { match };

        return matches.Ranking();
    }

    public static IEnumerable<Player> Ranking(this IReadOnlyCollection<Match> matches)
    {
        var players = new Dictionary<string, Player>();
        var lastMatch = true;

        foreach (var match in matches)
        {
            foreach (var point in match.Points())
            {
                players.AddIfNotExist(point);
                players[point.CommunityId].Points += point.Points;

                if (lastMatch)
                    players[point.CommunityId].LastMatchPoints += point.Points;
            }

            lastMatch = false;
        }

        foreach (var match in matches)
        {
            var lastMap = match.Maps.First();
            var statistic = lastMap.Statistic;
            if (statistic == null)
                continue;

            if (statistic.Draw())
            {
                foreach (var playerName in statistic.TeamA.Where(p => players.ContainsKey(p.CommunityId!)))
                    players[playerName.CommunityId!].Draw++;

                foreach (var playerName in statistic.TeamB.Where(p => players.ContainsKey(p.CommunityId!)))
                    players[playerName.CommunityId!].Draw++;

                continue;
            }

            var winners = statistic.Winners().ToHashSet();
            var losers = statistic.Losers().ToHashSet();

            foreach (var communityId in winners.Where(communityId => players.ContainsKey(communityId)))
                players[communityId].Wins++;

            foreach (var communityId in losers.Where(communityId => players.ContainsKey(communityId)))
                players[communityId].Loss++;
        }

        return players.Values.RankPlayers();
    }

    private static IEnumerable<MatchPoints> Points(this Match match)
    {
        var lastMap = match.Maps.First();
        var statistic = lastMap.Statistic;
        if (statistic == null)
            yield break;

        var draw = statistic.Draw();

        const int pointsToBeDistributed = 1_000;
        var percentageToWinners = draw ? 0.5m : 0.75m;
        var percentageToLosers = 1 - percentageToWinners;

        var amountOfMaps = match.Maps.Count;
        var pointsPerMapForWinners = pointsToBeDistributed * percentageToWinners / amountOfMaps;
        var pointsPerRoundForWinners = pointsPerMapForWinners / 2;
        var pointsPerMapForLosers = pointsToBeDistributed * percentageToLosers / amountOfMaps;
        var pointsPerRoundForLosers = pointsPerMapForLosers / 2;

        var winners = statistic.Winners().ToHashSet();
        var losers = statistic.Losers().ToHashSet();

        foreach (var half in match.Maps.Where(map => map.Statistic != null).SelectMany(map => map.Statistic!.Halves))
        {
            foreach (var player in half.Players.Where(player => !string.IsNullOrEmpty(player.CommunityId)))
            {
                if (winners.Contains(player.CommunityId!))
                    yield return player.Points(half.Players, pointsPerRoundForWinners);

                if (losers.Contains(player.CommunityId!))
                    yield return player.Points(half.Players, pointsPerRoundForLosers);
            }

            foreach (var infectedPlayer in half.InfectedPlayers.Where(infectedPlayer => !string.IsNullOrEmpty(infectedPlayer.CommunityId)))
            {
                if (winners.Contains(infectedPlayer.CommunityId!))
                    yield return infectedPlayer.Points(half.InfectedPlayers, pointsPerRoundForWinners);

                if (losers.Contains(infectedPlayer.CommunityId!))
                    yield return infectedPlayer.Points(half.InfectedPlayers, pointsPerRoundForLosers);
            }
        }
    }
}