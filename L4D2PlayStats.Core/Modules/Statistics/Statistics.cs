using System.Runtime.Serialization;
using Azure;
using Azure.Data.Tables;

namespace L4D2PlayStats.Core.Modules.Statistics;

public class Statistics : ITableEntity
{
    private string? _content;
    private L4D2PlayStats.Statistics? _statistic;

    public string Server
    {
        get => PartitionKey;
        set => PartitionKey = value;
    }

    public string FileName
    {
        get => RowKey;
        set => RowKey = value;
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
    public L4D2PlayStats.Statistics? Statistic => _statistic ??= L4D2PlayStats.Statistics.TryParse(Content!, out var statistic) ? statistic : null;

    public string PartitionKey { get; set; } = default!;
    public string RowKey { get; set; } = default!;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}