using L4D2PlayStats.Core.Modules.Matches;

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
        {
            foreach (var playerName in match.Winners())
                players.TryAdd(playerName)?.AddWin();

            foreach (var playerName in match.Losers())
                players.TryAdd(playerName)?.AddLoss();

            foreach (var team in match.Teams)
            foreach (var matchPlayer in team.Players)
            {
                var player = players.TryAdd(matchPlayer);
                if (player == null)
                    continue;

                player.Mvps += matchPlayer.MvpSiDamage;
                player.MvpsCommon += matchPlayer.MvpCommon;
            }
        }

        return players.Values.RankPlayers();
    }

    private static IEnumerable<PlayerName> Winners(this Match match)
    {
        var lastMap = match.Maps.Select(m => m.Statistic).FirstOrDefault();

        if (lastMap?.Scoring?.TeamA == null
            || lastMap.Scoring?.TeamB == null
            || lastMap.Scoring.TeamA.Score == lastMap.Scoring.TeamB.Score)
            yield break;

        var winners = lastMap.Scoring.TeamA.Score > lastMap.Scoring.TeamB.Score ? lastMap.TeamA : lastMap.TeamB;

        foreach (var playerName in winners)
            yield return playerName;
    }

    private static IEnumerable<PlayerName> Losers(this Match match)
    {
        var lastMap = match.Maps.Select(m => m.Statistic).FirstOrDefault();

        if (lastMap?.Scoring?.TeamA == null
            || lastMap.Scoring?.TeamB == null
            || lastMap.Scoring.TeamA.Score == lastMap.Scoring.TeamB.Score)
            yield break;

        var losers = lastMap.Scoring.TeamA.Score > lastMap.Scoring.TeamB.Score ? lastMap.TeamB : lastMap.TeamA;

        foreach (var playerName in losers)
            yield return playerName;
    }
}