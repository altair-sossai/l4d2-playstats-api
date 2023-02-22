using L4D2PlayStats.Core.Modules.Matches.Structures;

namespace L4D2PlayStats.Core.Modules.Matches.Extensions;

public static class InfectedPlayerExtensions
{
    public static MatchPoints Points(this InfectedPlayer infectedPlayer, List<InfectedPlayer> infectedPlayers, decimal points)
    {
        var percentages = new[]
        {
            infectedPlayer.DmgTotal(infectedPlayers),
            infectedPlayer.DmgSpit(infectedPlayers),
            infectedPlayer.DmgBoom(infectedPlayers)
        };

        var pointsPerMetric = points / percentages.Length;
        var sum = percentages.Select(p => p * pointsPerMetric).Sum();

        var matchPoints = new MatchPoints
        {
            CommunityId = infectedPlayer.CommunityId!,
            Name = infectedPlayer.PlayerName!,
            Points = sum
        };

        return matchPoints;
    }

    private static decimal DmgTotal(this InfectedPlayer infectedPlayer, IEnumerable<InfectedPlayer> infectedPlayers)
    {
        var dmgTotal = (decimal)infectedPlayer.DmgTotal;
        var sum = infectedPlayers.Sum(s => s.DmgTotal);
        var percentage = sum == 0 ? 0 : dmgTotal / sum;

        return percentage;
    }

    private static decimal DmgSpit(this InfectedPlayer infectedPlayer, IEnumerable<InfectedPlayer> infectedPlayers)
    {
        var dmgSpit = (decimal)infectedPlayer.DmgSpit;
        var sum = infectedPlayers.Sum(s => s.DmgSpit);
        var percentage = sum == 0 ? 0 : dmgSpit / sum;

        return percentage;
    }

    private static decimal DmgBoom(this InfectedPlayer infectedPlayer, IEnumerable<InfectedPlayer> infectedPlayers)
    {
        var dmgBoom = (decimal)infectedPlayer.DmgBoom;
        var sum = infectedPlayers.Sum(s => s.DmgBoom);
        var percentage = sum == 0 ? 0 : dmgBoom / sum;

        return percentage;
    }
}