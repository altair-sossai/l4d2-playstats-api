using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Azure;
using Azure.Data.Tables;
using L4D2PlayStats.Core.Modules.Statistics.Helpers;

namespace L4D2PlayStats.Core.Modules.Statistics;

public class Statistics : ITableEntity
{
    private string? _content;
    private string? _fileName;
    private L4D2PlayStats.Statistics? _statistic;

    public string Server
    {
        get => PartitionKey;
        set => PartitionKey = value;
    }

    public string? FileName
    {
        get => _fileName;
        set
        {
            _fileName = value;
            RowKey = StatisticsHelper.FileNameToRowKey(value)!;
        }
    }

    public int Round { get; set; }
    public int TeamSize { get; set; }
    public string? ConfigurationName { get; set; }
    public string? MapName { get; set; }

    public string? Content
    {
        get => _content;
        set
        {
            _content = value;
            _statistic = null;
        }
    }

    [IgnoreDataMember]
    [JsonIgnore]
    public L4D2PlayStats.Statistics? Statistic => _statistic ??= L4D2PlayStats.Statistics.TryParse(Content!, out var statistic) ? statistic : null;

    public int ScoreDifference => Math.Abs((Statistic?.Scoring?.TeamA?.Score ?? 0) - (Statistic?.Scoring?.TeamB?.Score ?? 00));

    public string PartitionKey { get; set; } = null!;
    public string RowKey { get; set; } = null!;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public void UpdateScore(int teamAScore, int teamBScore)
    {
        if (Statistic == null)
            throw new InvalidOperationException("Statistic is not initialized.");

        if (Statistic.Scoring == null)
            throw new InvalidOperationException("Scoring is not initialized in the statistic.");

        if (Statistic.Scoring.TeamA == null)
            throw new InvalidOperationException("Team A is not initialized in the statistic scoring.");

        if (Statistic.Scoring.TeamB == null)
            throw new InvalidOperationException("Team B is not initialized in the statistic scoring.");

        Statistic.Scoring.TeamA.Score = teamAScore;
        Statistic.Scoring.TeamB.Score = teamBScore;

        Content = Statistic.ToString();
    }
}