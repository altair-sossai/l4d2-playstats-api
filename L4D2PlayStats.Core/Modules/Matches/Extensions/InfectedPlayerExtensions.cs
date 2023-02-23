using L4D2PlayStats.Core.Modules.Matches.Structures;

namespace L4D2PlayStats.Core.Modules.Matches.Extensions;

public static class InfectedPlayerExtensions
{
    public static MatchPoints Points(this InfectedPlayer infectedPlayer, List<InfectedPlayer> infectedPlayers, decimal points)
    {
        var weights = new PointWeights
        (
            new PointWeight(infectedPlayer.DmgUpright, infectedPlayers.Select(p => p.DmgUpright), 3m),
            new PointWeight(infectedPlayer.HunterDpDmg, infectedPlayers.Select(p => p.HunterDpDmg)),
            new PointWeight(infectedPlayer.DeathCharges, infectedPlayers.Select(p => p.DeathCharges), 5m),
            new PointWeight(infectedPlayer.Booms, infectedPlayers.Select(p => p.Booms))
        );

        var calculatedPoints = weights.CalculatePoints(points);

        var matchPoints = new MatchPoints
        {
            CommunityId = infectedPlayer.CommunityId!,
            Name = infectedPlayer.PlayerName!,
            Points = calculatedPoints
        };

        return matchPoints;
    }
}