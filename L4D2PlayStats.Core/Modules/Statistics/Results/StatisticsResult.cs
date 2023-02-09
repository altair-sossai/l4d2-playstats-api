namespace L4D2PlayStats.Core.Modules.Statistics.Results;

public class StatisticsResult
{
    public string? Server { get; set; }
    public string? FileName { get; set; }
    public bool Completed => Statistic is { GameRound: { }, Halves.Count: 2, Scoring: { } };
    public L4D2PlayStats.Statistics? Statistic { get; set; }
}