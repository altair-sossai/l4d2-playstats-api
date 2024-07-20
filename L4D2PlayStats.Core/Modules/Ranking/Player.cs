using L4D2PlayStats.Core.Infrastructure.Structures;

namespace L4D2PlayStats.Core.Modules.Ranking;

public class Player
{
    private const int WinExperience = 80;
    private const int LossExperience = -40;
    private const int MvpsExperience = 10;
    private const int MvpsCommonExperience = 8;

    private readonly long _communityId;
    private decimal _gameExperience;
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
    public decimal Experience => GameExperience + Mvps * MvpsExperience + MvpsCommon * MvpsCommonExperience;
    public decimal? PreviousExperience { get; set; }
    public int Games { get; private set; }
    public int Wins { get; private set; }
    public int Loss { get; private set; }
    public decimal WinRate => Games == 0 ? 0 : Wins / (decimal)Games;
    public int Mvps { get; set; }
    public int MvpsCommon { get; set; }

    private decimal GameExperience
    {
        get => _gameExperience;
        set => _gameExperience = Math.Max(0, value);
    }

    public void AddWin()
    {
        Games++;
        Wins++;
        GameExperience += WinExperience;
    }

    public void AddLoss()
    {
        Games++;
        Loss++;
        GameExperience += LossExperience;
    }
}