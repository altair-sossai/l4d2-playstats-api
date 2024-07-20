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
        var previousExperience = new Dictionary<string, decimal>();

        foreach (var match in matches.Reverse())
        {
            previousExperience.Clear();

            foreach (var team in match.Teams)
            foreach (var matchPlayer in team.Players)
            {
                if (string.IsNullOrEmpty(matchPlayer.CommunityId)
                    || !players.ContainsKey(matchPlayer.CommunityId)
                    || previousExperience.ContainsKey(matchPlayer.CommunityId))
                    continue;

                previousExperience.Add(matchPlayer.CommunityId, players[matchPlayer.CommunityId].Experience);
            }

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

        foreach (var (communityId, experience) in previousExperience)
        {
            if (!players.TryGetValue(communityId, out var player))
                continue;

            player.PreviousExperience = experience;
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