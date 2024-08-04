using L4D2PlayStats.Core.Infrastructure.Structures;
using L4D2PlayStats.Core.Modules.Matches;

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
    public int RageQuit { get; set; }
    public decimal WinRate => Games == 0 ? 0 : Wins / (decimal)Games;

    /* Survivor */
    public int Died { get; set; }
    public int Incaps { get; set; }
    public int DmgTaken { get; set; }
    public int Common { get; set; }
    public int SiKilled { get; set; }
    public int SiDamage { get; set; }
    public int TankDamage { get; set; }
    public int RockEats { get; set; }
    public int WitchDamage { get; set; }
    public int Skeets { get; set; }
    public int Levels { get; set; }
    public int Crowns { get; set; }
    public int FfGiven { get; set; }

    /* Infected */
    public int DmgTotal { get; set; }
    public int DmgTank { get; set; }
    public int DmgSpit { get; set; }
    public int HunterDpDmg { get; set; }

    /* MVP and LVP */
    public int MvpSiDamage { get; set; }
    public int MvpCommon { get; set; }
    public int LvpFfGiven { get; set; }

    public void AppendInfo(Match.Player player)
    {
        Died += player.Died;
        Incaps += player.Incaps;
        DmgTaken += player.DmgTaken;
        Common += player.Common;
        SiKilled += player.SiKilled;
        SiDamage += player.SiDamage;
        TankDamage += player.TankDamage;
        RockEats += player.RockEats;
        WitchDamage += player.WitchDamage;
        Skeets += player.Skeets;
        Levels += player.Levels;
        Crowns += player.Crowns;
        FfGiven += player.FfGiven;

        /* Infected */
        DmgTotal += player.DmgTotal;
        DmgTank += player.DmgTank;
        DmgSpit += player.DmgSpit;
        HunterDpDmg += player.HunterDpDmg;

        /* MVP and LVP */
        MvpSiDamage += player.MvpSiDamage;
        MvpCommon += player.MvpCommon;
        LvpFfGiven += player.LvpFfGiven;
    }
}