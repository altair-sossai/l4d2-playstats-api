using L4D2PlayStats.Core.Modules.Matches;

namespace L4D2PlayStats.Core.Modules.Ranking.Extensions;

public static class PlayerExtensions
{
    extension(Dictionary<string, Player> players)
    {
        public Player? TryAdd(PlayerName playerName)
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

        public Player? TryAdd(Match.Player matchPlayer)
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

        public Player? TryAdd(L4D2PlayStats.Player statsPlayer)
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
    }

    extension(IEnumerable<Player> players)
    {
        public IEnumerable<Player> RankPlayers()
        {
            return players
                .OrderByDescending(o => o.Experience)
                .ThenByDescending(o => o.Wins)
                .ThenBy(o => o.Loss)
                .ThenByDescending(o => o.MvpSiDamage)
                .ThenByDescending(o => o.MvpCommon)
                .UpdatePosition();
        }

        private IEnumerable<Player> UpdatePosition()
        {
            var position = 1;

            foreach (var player in players)
            {
                player.Position = position++;
                yield return player;
            }
        }
    }
}