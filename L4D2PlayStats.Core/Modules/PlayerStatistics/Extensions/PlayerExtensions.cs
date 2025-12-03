namespace L4D2PlayStats.Core.Modules.PlayerStatistics.Extensions;

public static class PlayerExtensions
{
    extension(Dictionary<string, Player> players)
    {
        public Player? AddIfNotExist(L4D2PlayStats.Player playerStats)
        {
            if (string.IsNullOrEmpty(playerStats.CommunityId))
                return null;

            if (players.TryGetValue(playerStats.CommunityId, out var value))
                return value;

            var player = new Player
            {
                CommunityId = long.Parse(playerStats.CommunityId),
                Name = playerStats.PlayerName
            };

            players.Add(playerStats.CommunityId, player);

            return player;
        }

        public Player? AddIfNotExist(InfectedPlayer infectedPlayerStats)
        {
            if (string.IsNullOrEmpty(infectedPlayerStats.CommunityId))
                return null;

            if (players.TryGetValue(infectedPlayerStats.CommunityId, out var value))
                return value;

            var player = new Player
            {
                CommunityId = long.Parse(infectedPlayerStats.CommunityId),
                Name = infectedPlayerStats.PlayerName
            };

            players.Add(infectedPlayerStats.CommunityId, player);

            return player;
        }
    }
}