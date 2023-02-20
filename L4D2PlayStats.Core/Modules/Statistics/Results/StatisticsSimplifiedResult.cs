namespace L4D2PlayStats.Core.Modules.Statistics.Results;

public class StatisticsSimplifiedResult
{
    public string? Server { get; set; }
    public string? FileName { get; set; }
    public string? StatisticId { get; set; }
    public GameRound? GameRound { get; set; }
    public Scoring? Scoring { get; set; }
    public List<PlayerName>? PlayerNames { get; set; }
    public List<PlayerName>? TeamA { get; set; }
    public List<PlayerName>? TeamB { get; set; }
	public DateTime? MapStart { get; set; }
    public DateTime? MapEnd { get; set; }
    public TimeSpan? MapElapsed { get; set; }
}