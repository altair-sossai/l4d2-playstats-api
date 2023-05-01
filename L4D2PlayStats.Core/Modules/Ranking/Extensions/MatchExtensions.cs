using L4D2PlayStats.Core.Modules.Matches;

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

        return players.Values.RankPlayers();
    }
}