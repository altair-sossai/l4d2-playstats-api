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
    public string PartitionKey { get; set; } = null!;
    public string RowKey { get; set; } = null!;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}