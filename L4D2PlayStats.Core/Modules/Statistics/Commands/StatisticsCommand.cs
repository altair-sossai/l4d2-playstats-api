namespace L4D2PlayStats.Core.Modules.Statistics.Commands;

public class StatisticsCommand
{
    private L4D2PlayStats.Statistics? _statistics;

    public string? FileName { get; set; }

    public string? Content
    {
        get;
        set
        {
            field = value;
            _statistics = L4D2PlayStats.Statistics.TryParse(value!, out var statistics) ? statistics : null;
        }
    }

    public int Round => _statistics?.GameRound?.Round ?? 0;
    public int TeamSize => _statistics?.GameRound?.TeamSize ?? 0;
    public string? ConfigurationName => _statistics?.GameRound?.ConfigurationName;
    public string? MapName => _statistics?.GameRound?.MapName;
}