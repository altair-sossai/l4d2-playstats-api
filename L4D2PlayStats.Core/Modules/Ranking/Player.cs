using L4D2PlayStats.Core.Infrastructure.Structures;
using L4D2PlayStats.Core.Modules.Matches;
using L4D2PlayStats.Core.Modules.Ranking.Structures;

namespace L4D2PlayStats.Core.Modules.Ranking;

public class Player
{
    private readonly long _communityId;
    private decimal _experience;
    private string? _name;
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

    public string? Name
    {
        get => _name;
        set
        {
            _name = value;

            if (string.IsNullOrEmpty(value))
                return;

            var previousName = PreviousNames.FirstOrDefault(pn => pn.Name.Equals(value, StringComparison.CurrentCultureIgnoreCase));

            if (previousName == null)
            {
                previousName = new PreviousName(value);
                PreviousNames.Add(previousName);
            }

            previousName.TimesUsed++;

            PreviousNames.Sort((a, b) => b.TimesUsed - a.TimesUsed);
        }
    }

    public string MainName => PreviousNames.FirstOrDefault()?.Name ?? Name ?? "Unknown";

    public string MostUsedNames => string.Join(" | ", PreviousNames.Take(3).Select(pn => pn.Name));

    public List<PreviousName> PreviousNames { get; } = [];

    public decimal Experience
    {
        get => _experience;
        set => _experience = Math.Max(0, value);
    }

    public decimal? PreviousExperience { get; set; }
    public decimal? ExperienceDifference => PreviousExperience == null ? null : Experience - PreviousExperience;
    public int Wins { get; set; }
    public int Loss { get; set; }
    public int RageQuit { get; set; }
    public int Punishment { get; set; }
    public int Games => Wins + Loss;
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
    public int SkeetsMelee { get; set; }
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

    public Dictionary<long, AnotherPlayer> Anothers { get; } = [];

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
        SkeetsMelee += player.SkeetsMelee;
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

    public void WonWith(Player player)
    {
        if (player.CommunityId == CommunityId)
            return;

        if (!Anothers.ContainsKey(player.CommunityId))
            Anothers.Add(player.CommunityId, new AnotherPlayer(player));

        var anotherPlayer = Anothers[player.CommunityId];

        anotherPlayer.Name = player.Name;
        anotherPlayer.WinsWith++;
    }

    public void LostWith(Player player)
    {
        if (player.CommunityId == CommunityId)
            return;

        if (!Anothers.ContainsKey(player.CommunityId))
            Anothers.Add(player.CommunityId, new AnotherPlayer(player));

        var anotherPlayer = Anothers[player.CommunityId];

        anotherPlayer.Name = player.Name;
        anotherPlayer.LossWith++;
    }

    public void WonAgainst(Player player)
    {
        if (player.CommunityId == CommunityId)
            return;

        if (!Anothers.ContainsKey(player.CommunityId))
            Anothers.Add(player.CommunityId, new AnotherPlayer(player));

        var anotherPlayer = Anothers[player.CommunityId];

        anotherPlayer.Name = player.Name;
        anotherPlayer.WinsAgainst++;
    }

    public void LostAgainst(Player player)
    {
        if (player.CommunityId == CommunityId)
            return;

        if (!Anothers.ContainsKey(player.CommunityId))
            Anothers.Add(player.CommunityId, new AnotherPlayer(player));

        var anotherPlayer = Anothers[player.CommunityId];

        anotherPlayer.Name = player.Name;
        anotherPlayer.LossAgainst++;
    }

    public class AnotherPlayer(Player playerName)
    {
        public string? Name { get; set; } = playerName.Name;
        public int WinsWith { get; set; }
        public int LossWith { get; set; }
        public int WinsAgainst { get; set; }
        public int LossAgainst { get; set; }
    }
}