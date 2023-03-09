using Azure;
using Azure.Data.Tables;

namespace L4D2PlayStats.Core.Modules.Server;

public class Server : ITableEntity
{
    private string? _configurationName;
    private HashSet<string>? _configurationNames;

    public string Id
    {
        get => RowKey;
        set => RowKey = value;
    }

    public string? DisplayName { get; set; }
    public string? Secret { get; set; }

    public string? ConfigurationName
    {
        get => _configurationName;
        set
        {
            _configurationName = value;
            _configurationNames = value?.Split(';').ToHashSet();
        }
    }

    public string PartitionKey { get; set; } = "shared";
    public string RowKey { get; set; } = default!;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public bool RankingConfiguration(string? configurationName)
    {
        return !string.IsNullOrEmpty(configurationName)
               && _configurationNames != null
               && _configurationNames.Count != 0
               && _configurationNames.Contains(configurationName);
    }
}