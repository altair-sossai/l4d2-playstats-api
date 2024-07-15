using L4D2PlayStats.Core.Infrastructure.Structures;
using L4D2PlayStats.Core.Modules.PlayerStatistics.Structures;

namespace L4D2PlayStats.Core.Modules.PlayerStatistics;

public class Player
{
    private readonly long _communityId;
    private SteamIdentifiers _steamIdentifiers;

    public long CommunityId
    {
        get => _communityId;
        init
        {
            _communityId = value;
            SteamIdentifiers.TryParse(value.ToString(), out _steamIdentifiers);
        }
    }

    public string? SteamId => _steamIdentifiers.SteamId;
    public string? Steam3 => _steamIdentifiers.Steam3;
    public string? ProfileUrl => _steamIdentifiers.ProfileUrl;

    public string? Name { get; set; }

    public SurvivorStats SurvivorStats { get; } = new();
    public InfectedStats InfectedStats { get; } = new();
}