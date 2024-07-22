using L4D2PlayStats.Core.Infrastructure.Structures;

namespace L4D2PlayStats.Core.Modules.Ranking;

public class Player
{
    private readonly long _communityId;
    private decimal _experience;
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

    public decimal Experience
    {
        get => _experience;
        set => _experience = Math.Max(0, value);
    }

    public decimal? PreviousExperience { get; set; }
    public decimal? ExperienceDifference => PreviousExperience == null ? null : Experience - PreviousExperience;
    public int Games { get; set; }
    public int Wins { get; set; }
    public int Loss { get; set; }
    public int Mvps { get; set; }
    public int MvpsCommon { get; set; }
    public decimal WinRate => Games == 0 ? 0 : Wins / (decimal)Games;
}