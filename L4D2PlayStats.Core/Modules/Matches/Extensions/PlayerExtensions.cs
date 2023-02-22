using L4D2PlayStats.Core.Modules.Matches.Structures;

namespace L4D2PlayStats.Core.Modules.Matches.Extensions;

public static class PlayerExtensions
{
    public static MatchPoints Points(this Player player, List<Player> players, decimal points)
    {
        var percentages = new[]
        {
            player.Common(players),
            player.SiKilled(players),
            player.SiDamage(players),
            player.TankDamage(players),
            player.WitchDamage(players)
        };

        var pointsPerMetric = points / percentages.Length;
        var sum = percentages.Select(p => p * pointsPerMetric).Sum();

        var matchPoints = new MatchPoints
        {
            CommunityId = player.CommunityId!,
            Name = player.PlayerName!,
            Points = sum
        };

        return matchPoints;
    }

    private static decimal Common(this Player player, IEnumerable<Player> players)
    {
        var common = (decimal)player.Common;
        var sum = players.Sum(s => s.Common);
        var percentage = sum == 0 ? 0 : common / sum;

        return percentage;
    }

    private static decimal SiKilled(this Player player, IEnumerable<Player> players)
    {
        var siKilled = (decimal)player.SiKilled;
        var sum = players.Sum(s => s.SiKilled);
        var percentage = sum == 0 ? 0 : siKilled / sum;

        return percentage;
    }

    private static decimal SiDamage(this Player player, IEnumerable<Player> players)
    {
        var siDamage = (decimal)player.SiDamage;
        var sum = players.Sum(s => s.SiDamage);
        var percentage = sum == 0 ? 0 : siDamage / sum;

        return percentage;
    }

    private static decimal TankDamage(this Player player, IEnumerable<Player> players)
    {
        var tankDamage = (decimal)player.TankDamage;
        var sum = players.Sum(s => s.TankDamage);
        var percentage = sum == 0 ? 0 : tankDamage / sum;

        return percentage;
    }

    private static decimal WitchDamage(this Player player, IEnumerable<Player> players)
    {
        var witchDamage = (decimal)player.WitchDamage;
        var sum = players.Sum(s => s.WitchDamage);
        var percentage = sum == 0 ? 0 : witchDamage / sum;

        return percentage;
    }
}