namespace L4D2PlayStats.Core.Modules.Statistics.Results;

public class UploadResult
{
    public UploadResult(Statistics statistics)
    {
        FileName = statistics.FileName;
        MustBeDeleted = statistics.Statistic is { GameRound: { }, Halves.Count: 2, Scoring: { } }
                        && statistics.Statistic.Halves.All(half => half is { RoundHalf: { }, Progress: { } });
    }

    public string? FileName { get; }
    public bool MustBeDeleted { get; }
}