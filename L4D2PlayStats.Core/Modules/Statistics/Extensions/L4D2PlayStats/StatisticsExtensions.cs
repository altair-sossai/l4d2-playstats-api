namespace L4D2PlayStats.Core.Modules.Statistics.Extensions.L4D2PlayStats;

public static class StatisticsExtensions
{
    public static bool Draw(this global::L4D2PlayStats.Statistics statistics)
    {
        var scoring = statistics.Scoring;
        var teamA = scoring?.TeamA;
        var teamB = scoring?.TeamB;

        return teamA?.Score == teamB?.Score;
    }

    public static IEnumerable<PlayerName> Winners(this global::L4D2PlayStats.Statistics statistics)
    {
        var scoring = statistics.Scoring;
        var teamA = scoring?.TeamA;
        var teamB = scoring?.TeamB;

        if (teamA == null || teamB == null)
            return Enumerable.Empty<PlayerName>();

        return teamA.Score > teamB.Score ? statistics.TeamA : statistics.TeamB;
    }

    public static IEnumerable<PlayerName> Losers(this global::L4D2PlayStats.Statistics statistics)
    {
        var scoring = statistics.Scoring;
        var teamA = scoring?.TeamA;
        var teamB = scoring?.TeamB;

        if (teamA == null || teamB == null)
            return Enumerable.Empty<PlayerName>();

        return teamA.Score > teamB.Score ? statistics.TeamB : statistics.TeamA;
    }
}