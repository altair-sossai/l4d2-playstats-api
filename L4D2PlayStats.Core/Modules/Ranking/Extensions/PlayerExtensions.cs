namespace L4D2PlayStats.Core.Modules.Ranking.Extensions;

public static class PlayerExtensions
{
    public static IEnumerable<Player> RankPlayers(this IEnumerable<Player> players)
    {
        return players
            .OrderByDescending(o => o.Points)
            .UpdatePosition();
    }

    private static IEnumerable<Player> UpdatePosition(this IEnumerable<Player> players)
    {
        var position = 1;

        foreach (var player in players)
        {
            player.Position = position++;
            yield return player;
        }
    }
}