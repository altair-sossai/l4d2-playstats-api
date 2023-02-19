using Azure;
using Azure.Data.Tables;

namespace L4D2PlayStats.Core.Modules.Server;

public class Server : ITableEntity
{
    public string Id
    {
        get => RowKey;
        set => RowKey = value;
    }

    public string? DisplayName { get; set; }
    public string? Secret { get; set; }
    public string PartitionKey { get; set; } = "shared";
    public string RowKey { get; set; } = default!;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}