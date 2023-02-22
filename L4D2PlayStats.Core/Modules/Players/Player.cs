using Azure;
using Azure.Data.Tables;
using L4D2PlayStats.Core.Contexts.Steam.Structures;

namespace L4D2PlayStats.Core.Modules.Players;

public class Player : ITableEntity
{
    private long _communityId;
    private SteamIdentifiers _steamIdentifiers;

    public long CommunityId
    {
        get => _communityId;
        set
        {
            _communityId = value;
            SteamIdentifiers.TryParse(value.ToString(), out _steamIdentifiers);
        }
    }

    public string? SteamId => _steamIdentifiers.SteamId;
    public string? Steam3 => _steamIdentifiers.Steam3;
    public string? ProfileUrl => _steamIdentifiers.ProfileUrl;

    public int Position { get; set; }
    public string? Name { get; set; }
    public decimal Points { get; set; }

    public string PartitionKey { get; set; } = default!;

    public string RowKey
    {
        get => CommunityId.ToString();
        set => CommunityId = long.TryParse(value, out var communityId) ? communityId : 0;
    }

    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}