using L4D2PlayStats.Core.Modules.Matches.Structures;

namespace L4D2PlayStats.Core.Modules.Matches.Extensions;

public static class PlayerExtensions
{
    public static MatchPoints Points(this Player player, List<Player> players, decimal points)
    {
        var weights = new PointWeights
        (
            new PointWeight(player.Common, players.Select(p => p.Common), 1.5m),
            new PointWeight(player.SiKilled, players.Select(p => p.SiKilled), 1.5m),
            new PointWeight(player.SiDamage, players.Select(p => p.SiDamage), 3),
            new PointWeight(player.TankDamage, players.Select(p => p.TankDamage), 1.5m),
            new PointWeight(player.WitchDamage, players.Select(p => p.WitchDamage)),
            new PointWeight(player.Clears, players.Select(p => p.Clears), 1.5m)
        );

        var calculatedPoints = weights.CalculatePoints(points);

        var matchPoints = new MatchPoints
        {
            CommunityId = player.CommunityId!,
            Name = player.PlayerName!,
            Points = calculatedPoints
        };

        return matchPoints;
    }
}