using L4D2PlayStats.Core.Modules.Matches;

namespace L4D2PlayStats.Core.Modules.Ranking.Extensions;

public static class PlayerExtensions
{
    public static Player? TryAdd(this Dictionary<string, Player> players, PlayerName playerName)
    {
        var communityId = playerName.CommunityId;

        if (string.IsNullOrEmpty(communityId))
            return null;

        if (players.TryGetValue(communityId, out var player))
        {
            player.Name = playerName.Name;

            return player;
        }

        players.Add(communityId, new Player
        {
            CommunityId = long.Parse(communityId),
            Name = playerName.Name
        });

        return players[communityId];
    }

    public static Player? TryAdd(this Dictionary<string, Player> players, Match.Player matchPlayer)
    {
        var communityId = matchPlayer.CommunityId;

        if (string.IsNullOrEmpty(communityId))
            return null;

        if (players.TryGetValue(communityId, out var player))
            return player;

        players.Add(communityId, new Player
        {
            CommunityId = long.Parse(communityId),
            Name = matchPlayer.Name
        });

        return players[communityId];
    }

    public static Player? TryAdd(this Dictionary<string, Player> players, L4D2PlayStats.Player statsPlayer)
    {
        var communityId = statsPlayer.CommunityId;

        if (string.IsNullOrEmpty(communityId))
            return null;

        if (players.TryGetValue(communityId, out var player))
            return player;

        players.Add(communityId, new Player
        {
            CommunityId = long.Parse(communityId),
            Name = statsPlayer.PlayerName
        });

        return players[communityId];
    }

    public static IEnumerable<Player> RankPlayers(this IEnumerable<Player> players)
    {
        return players
            .OrderByDescending(o => o.Experience)
            .ThenByDescending(o => o.Wins)
            .ThenBy(o => o.Loss)
            .ThenByDescending(o => o.MvpSiDamage)
            .ThenByDescending(o => o.MvpCommon)
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