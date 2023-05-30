using L4D2PlayStats.Core.Modules.Ranking.Structures;

namespace L4D2PlayStats.Core.Modules.Ranking.Extensions;

public static class PlayerPointsExtensions
{
    public static void AddOrUpdate(this Dictionary<string, PlayerPoints> points, PlayerPoints player)
    {
        var communityId = player.CommunityId;

        if (string.IsNullOrEmpty(communityId))
            return;

        points.TryAdd(communityId, player);

        points[communityId].Name = player.Name;
    }
}