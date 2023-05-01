using L4D2PlayStats.Core.Modules.Ranking.Structures;

namespace L4D2PlayStats.Core.Modules.Ranking.Extensions;

public static class PlayerExtensions
{
    public static Player? AddPoints(this Dictionary<string, Player> players, PlayerPoints points)
    {
        var communityId = points.CommunityId!;

        if (string.IsNullOrEmpty(communityId))
            return null;

        players.TryAdd(communityId, new Player
        {
            CommunityId = long.Parse(communityId),
            Name = points.Name
        });

        var player = players[communityId];

        player.Name = points.Name;
        player.Points += points.Points;

        if (points.Winner)
            player.Wins++;

        if (points.Loser)
            player.Loss++;

        if (points.Draw)
            player.Draw++;

        if (points.Rage)
            player.Rage++;

        return player;
    }


    public static IEnumerable<Player> RankPlayers(this IEnumerable<Player> players)
    {
        return players
            .OrderByDescending(o => o.Points)
            .ThenByDescending(o => o.Wins)
            .ThenBy(o => o.Loss)
            .ThenBy(o => o.Draw)
            .ThenBy(o => o.Rage)
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