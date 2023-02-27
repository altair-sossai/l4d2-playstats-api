using L4D2PlayStats.Core.Modules.Matches.Structures;

namespace L4D2PlayStats.Core.Modules.Matches.Extensions;

public static class PlayerExtensions
{
    public static MatchPoints Points(this Player player, List<Player> players, decimal points)
    {
        var weights = new PointWeights
        (
            /* 2.0m Weight */
            new PointWeight(player.Common, players.Select(p => p.Common), 0.75m),
            new PointWeight(player.SiKilled, players.Select(p => p.SiKilled), 1.25m),
            /* 4.0m Weight */
            new PointWeight(player.SiDamage, players.Select(p => p.SiDamage), 2m),
            new PointWeight(player.TankDamage, players.Select(p => p.TankDamage), 1.5m),
            new PointWeight(player.WitchDamage, players.Select(p => p.WitchDamage), 0.5m),
            /* 1.0m Weight */
            new PointWeight(player.Clears, players.Select(p => p.Clears), 0.1m),
            new PointWeight(player.Crowns, players.Select(p => p.Crowns), 0.1m),
            new PointWeight(player.Skeets, players.Select(p => p.Skeets), 0.1m),
            new PointWeight(player.SkeetsMelee, players.Select(p => p.SkeetsMelee), 0.1m),
            new PointWeight(player.RockSkeets, players.Select(p => p.RockSkeets), 0.1m),
            new PointWeight(player.Levels, players.Select(p => p.Levels), 0.1m),
            new PointWeight(player.DeadStops, players.Select(p => p.DeadStops), 0.1m),
            new PointWeight(player.TongueCuts, players.Select(p => p.TongueCuts), 0.1m),
            new PointWeight(player.Shoves, players.Select(p => p.Shoves), 0.1m),
            new PointWeight(player.Pops, players.Select(p => p.Pops), 0.1m)
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