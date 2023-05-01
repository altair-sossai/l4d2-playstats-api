using L4D2PlayStats.Core.Modules.Matches;
using L4D2PlayStats.Core.Modules.Ranking.Structures;

namespace L4D2PlayStats.Core.Modules.Ranking.Extensions;

public static class MatchExtensions
{
    public static IEnumerable<Player> Ranking(this Match match)
    {
        var matches = new[] { match };

        return matches.Ranking();
    }

    public static IEnumerable<Player> Ranking(this IEnumerable<Match> matches)
    {
        var players = new Dictionary<string, Player>();

        foreach (var match in matches.Reverse())
        foreach (var points in match.Points())
        {
            var player = players.AddPoints(points);

            if (player == null)
                continue;

            player.LastMatchPoints = points.Points;
        }

        return players.Values.RankPlayers();
    }

    private static IEnumerable<PlayerPoints> Points(this Match match)
    {
        var points = new Dictionary<string, PlayerPoints>();

        foreach (var point in match.RageQuit())
            points.AddOrUpdate(point);

        foreach (var point in match.Tied())
            points.AddOrUpdate(point);

        foreach (var point in match.Winners())
            points.AddOrUpdate(point);

        foreach (var point in match.Losers())
            points.AddOrUpdate(point);

        return points.Values;
    }

    private static IEnumerable<PlayerPoints> RageQuit(this Match match)
    {
        var statistics = match.Maps.Select(m => m.Statistic).ToList();

        var firstMap = statistics.LastOrDefault();
        if (firstMap == null)
            yield break;

        var lastMap = statistics.FirstOrDefault();
        if (lastMap == null)
            yield break;

        var firstMapPlayers = firstMap.TeamA.Concat(firstMap.TeamB);
        var lastMapPlayers = lastMap.TeamA.Concat(lastMap.TeamB).Select(p => p.CommunityId!).ToHashSet();

        foreach (var playerName in firstMapPlayers.Where(p => !lastMapPlayers.Contains(p.CommunityId ?? string.Empty)))
            yield return PlayerPoints.RageQuit(playerName);
    }

    private static IEnumerable<PlayerPoints> Tied(this Match match)
    {
        var lastMap = match.Maps.Select(m => m.Statistic).FirstOrDefault();

        if (lastMap?.Scoring?.TeamA == null
            || lastMap.Scoring?.TeamB == null
            || lastMap.Scoring.TeamA.Score != lastMap.Scoring.TeamB.Score)
            yield break;

        foreach (var playerName in lastMap.TeamA.Concat(lastMap.TeamB))
            yield return PlayerPoints.Tied(playerName);
    }

    private static IEnumerable<PlayerPoints> Winners(this Match match)
    {
        var lastMap = match.Maps.Select(m => m.Statistic).FirstOrDefault();

        if (lastMap?.Scoring?.TeamA == null
            || lastMap.Scoring?.TeamB == null
            || lastMap.Scoring.TeamA.Score == lastMap.Scoring.TeamB.Score)
            yield break;

        var winners = lastMap.Scoring.TeamA.Score > lastMap.Scoring.TeamB.Score ? lastMap.TeamA : lastMap.TeamB;

        foreach (var playerName in winners)
            yield return PlayerPoints.Win(playerName);
    }

    private static IEnumerable<PlayerPoints> Losers(this Match match)
    {
        var lastMap = match.Maps.Select(m => m.Statistic).FirstOrDefault();

        if (lastMap?.Scoring?.TeamA == null
            || lastMap.Scoring?.TeamB == null
            || lastMap.Scoring.TeamA.Score == lastMap.Scoring.TeamB.Score)
            yield break;

        var losers = lastMap.Scoring.TeamA.Score > lastMap.Scoring.TeamB.Score ? lastMap.TeamB : lastMap.TeamA;

        foreach (var playerName in losers)
            yield return PlayerPoints.Lost(playerName);
    }
}