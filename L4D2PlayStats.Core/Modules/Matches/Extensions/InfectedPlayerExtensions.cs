using L4D2PlayStats.Core.Modules.Matches.Structures;

namespace L4D2PlayStats.Core.Modules.Matches.Extensions;

public static class InfectedPlayerExtensions
{
    public static MatchPoints Points(this InfectedPlayer infectedPlayer, List<InfectedPlayer> infectedPlayers, decimal points)
    {
        var percentages = new[]
        {
            infectedPlayer.DmgUpright(infectedPlayers) * 0.3m,
            infectedPlayer.HunterDpDmg(infectedPlayers) * 0.1m,
            infectedPlayer.DeathCharges(infectedPlayers) * 0.5m,
            infectedPlayer.Booms(infectedPlayers) * 0.1m
        };

        var pointsPerMetric = points / 1m;
        var sum = percentages.Select(p => p * pointsPerMetric).Sum();

        var matchPoints = new MatchPoints
        {
            CommunityId = infectedPlayer.CommunityId!,
            Name = infectedPlayer.PlayerName!,
            Points = sum
        };

        return matchPoints;
    }

    private static decimal DmgUpright(this InfectedPlayer infectedPlayer, IEnumerable<InfectedPlayer> infectedPlayers)
    {
        var dmgUpright = (decimal)infectedPlayer.DmgUpright;
        var sum = infectedPlayers.Sum(s => s.DmgUpright);
        var percentage = sum == 0 ? 0 : dmgUpright / sum;

        return percentage;
    }

    private static decimal HunterDpDmg(this InfectedPlayer infectedPlayer, IEnumerable<InfectedPlayer> infectedPlayers)
    {
        var hunterDpDmg = (decimal)infectedPlayer.HunterDpDmg;
        var sum = infectedPlayers.Sum(s => s.HunterDpDmg);
        var percentage = sum == 0 ? 0 : hunterDpDmg / sum;

        return percentage;
    }

    private static decimal DeathCharges(this InfectedPlayer infectedPlayer, IEnumerable<InfectedPlayer> infectedPlayers)
    {
        var deathCharges = (decimal)infectedPlayer.DeathCharges;
        var sum = infectedPlayers.Sum(s => s.DeathCharges);
        var percentage = sum == 0 ? 0 : deathCharges / sum;

        return percentage;
    }

    private static decimal Booms(this InfectedPlayer infectedPlayer, IEnumerable<InfectedPlayer> infectedPlayers)
    {
        var booms = (decimal)infectedPlayer.Booms;
        var sum = infectedPlayers.Sum(s => s.Booms);
        var percentage = sum == 0 ? 0 : booms / sum;

        return percentage;
    }
}