using Azure;
using Azure.Data.Tables;

namespace L4D2PlayStats.Core.Modules.Punishments;

public class Punishment : ITableEntity
{
    public string Server
    {
        get => PartitionKey;
        set => PartitionKey = value;
    }

    public string CommunityId
    {
        get => RowKey;
        set => RowKey = value;
    }

    public int LostExperiencePoints { get; set; }
    public string PartitionKey { get; set; } = default!;
    public string RowKey { get; set; } = default!;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}