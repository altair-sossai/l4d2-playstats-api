using Azure;
using Azure.Data.Tables;
using L4D2PlayStats.Modules.Auth.Users.ValueObjects;

namespace L4D2PlayStats.Modules.Auth.Users;

public class User : ITableEntity
{
    public string Id
    {
        get => RowKey;
        set => RowKey = value;
    }

    public string? DisplayName { get; set; }
    public string? Steam { get; set; }
    public string? Secret { get; set; }
    public string PartitionKey { get; set; } = "shared";
    public string RowKey { get; set; } = default!;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public UserInfo Info()
    {
        return new UserInfo
        {
            Id = Id,
            DisplayName = DisplayName,
            Steam = Steam
        };
    }
}