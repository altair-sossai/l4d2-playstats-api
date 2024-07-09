using L4D2PlayStats.Core.Contexts.Steam.Structures;

namespace L4D2PlayStats.Core.Modules.Ranking;

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
    public int Position { get; set; }
    public string? Name { get; set; }
    public int Games { get; set; }
    public int Wins { get; set; }
    public int Mvps { get; set; }
    public int Loss { get; set; }
}