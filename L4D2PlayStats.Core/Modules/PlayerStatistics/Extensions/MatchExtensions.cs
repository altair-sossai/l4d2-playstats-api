using L4D2PlayStats.Core.Modules.Matches;

namespace L4D2PlayStats.Core.Modules.PlayerStatistics.Extensions;

public static class MatchExtensions
{
    public static IEnumerable<Player> PlayerStatistics(this IEnumerable<Match> matches)
    {
        var players = new Dictionary<string, Player>();

        foreach (var match in matches)
        foreach (var half in match.Maps.Where(statistic => statistic.Statistic != null).SelectMany(statistic => statistic.Statistic!.Halves))
        {
            foreach (var playerStats in half.Players)
            {
                var player = players.AddIfNotExist(playerStats);
                if (player == null)
                    continue;

                player.SurvivorStats.Common += playerStats.Common;
                player.SurvivorStats.SiKilled += playerStats.SiKilled;
                player.SurvivorStats.SiDamage += playerStats.SiDamage;
            }

            foreach (var infectedPlayerStats in half.InfectedPlayers)
            {
                var player = players.AddIfNotExist(infectedPlayerStats);
                if (player == null)
                    continue;

                player.InfectedStats.DmgTotal += infectedPlayerStats.DmgTotal;
            }
        }

        return players.Values;
    }
}