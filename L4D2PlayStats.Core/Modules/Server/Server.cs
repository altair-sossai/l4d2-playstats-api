using System.Text.RegularExpressions;
using Azure;
using Azure.Data.Tables;

namespace L4D2PlayStats.Core.Modules.Server;

public class Server : ITableEntity
{
    private string? _configurationName;
    private Regex? _configurationNameRegex;

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

            if (!string.IsNullOrEmpty(_configurationName))
                _configurationNameRegex = new Regex(_configurationName);
        }
    }

    public string PartitionKey { get; set; } = "shared";
    public string RowKey { get; set; } = default!;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public bool RankingConfiguration(string? configurationName)
    {
        return !string.IsNullOrEmpty(configurationName)
               && _configurationNameRegex != null
               && _configurationNameRegex.IsMatch(configurationName);
    }
}