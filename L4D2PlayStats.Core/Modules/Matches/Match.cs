using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using L4D2PlayStats.Core.Modules.Campaigns;

namespace L4D2PlayStats.Core.Modules.Matches;

public class Match(Campaign campaign, Scoring.Team teamA, IEnumerable<PlayerName> playersA, Scoring.Team teamB, IEnumerable<PlayerName> playersB)
{
    public int TeamSize { get; internal init; }
    public DateTime MatchStart { get; internal set; }
    public DateTime? MatchEnd { get; internal init; }
    public TimeSpan? MatchElapsed => MatchEnd - MatchStart;
    public string? Campaign { get; } = campaign.Name;
    public List<Team> Teams { get; } = [new('A', teamA, playersA), new('B', teamB, playersB)];
    public List<string> Statistics { get; } = [];

    [IgnoreDataMember]
    [JsonIgnore]
    public List<Statistics.Statistics> Maps { get; } = [];

    [IgnoreDataMember]
    [JsonIgnore]
    public IEnumerable<L4D2PlayStats.Player>? FirstRoundPlayers => Maps.LastOrDefault()?.Statistic?.Halves.SelectMany(half => half.Players);

    [IgnoreDataMember]
    [JsonIgnore]
    public IEnumerable<L4D2PlayStats.Player>? LastRoundPlayers => Maps.FirstOrDefault()?.Statistic?.Halves.SelectMany(half => half.Players);

    public bool Competitive => Maps.Count >= 3 && TeamSize == 4;

    public void Add(Statistics.Statistics statistic)
    {
        Statistics.Add(statistic.RowKey);
        Maps.Add(statistic);
        UpdateTeamStats(statistic);
    }

    private void UpdateTeamStats(Statistics.Statistics statistic)
    {
        foreach (var team in Teams)
            team.UpdateStats(statistic);
    }

    public class Team
    {
        private readonly Scoring.Team _team;

        public Team(char code, Scoring.Team team, IEnumerable<PlayerName> players)
        {
            Code = code;
            _team = team;
            Players = players.Select(playerName => new Player(playerName, this)).ToList();
        }

        public char Code { get; }
        public int Score => _team.Score;
        public List<Player> Players { get; }

        /* Survivor */
        public int Died => Players.Select(p => p.Died).DefaultIfEmpty(0).Sum();
        public int Incaps => Players.Select(p => p.Incaps).DefaultIfEmpty(0).Sum();
        public int DmgTaken => Players.Select(p => p.DmgTaken).DefaultIfEmpty(0).Sum();
        public int Common => Players.Select(p => p.Common).DefaultIfEmpty(0).Sum();
        public int SiKilled => Players.Select(p => p.SiKilled).DefaultIfEmpty(0).Sum();
        public int SiDamage => Players.Select(p => p.SiDamage).DefaultIfEmpty(0).Sum();
        public int TankDamage => Players.Select(p => p.TankDamage).DefaultIfEmpty(0).Sum();
        public int RockEats => Players.Select(p => p.RockEats).DefaultIfEmpty(0).Sum();
        public int WitchDamage => Players.Select(p => p.WitchDamage).DefaultIfEmpty(0).Sum();
        public int Skeets => Players.Select(p => p.Skeets).DefaultIfEmpty(0).Sum();
        public int Levels => Players.Select(p => p.Levels).DefaultIfEmpty(0).Sum();
        public int Crowns => Players.Select(p => p.Crowns).DefaultIfEmpty(0).Sum();
        public int FfGiven => Players.Select(p => p.FfGiven).DefaultIfEmpty(0).Sum();

        /* Infected */
        public int DmgTotal => Players.Select(p => p.DmgTotal).DefaultIfEmpty(0).Sum();
        public int DmgTank => Players.Select(p => p.DmgTank).DefaultIfEmpty(0).Sum();
        public int DmgSpit => Players.Select(p => p.DmgSpit).DefaultIfEmpty(0).Sum();
        public int HunterDpDmg => Players.Select(p => p.HunterDpDmg).DefaultIfEmpty(0).Sum();

        /* MVP and LVP */
        public int MvpSiDamage => Players.Select(p => p.MvpSiDamage).DefaultIfEmpty(0).Sum();
        public int MvpCommon => Players.Select(p => p.MvpCommon).DefaultIfEmpty(0).Sum();
        public int LvpFfGiven => Players.Select(p => p.LvpFfGiven).DefaultIfEmpty(0).Sum();

        public void UpdateStats(Statistics.Statistics statistic)
        {
            if (statistic.Statistic == null)
                return;

            UpdateStats(statistic.Statistic);
        }

        private void UpdateStats(L4D2PlayStats.Statistics statistic)
        {
            foreach (var currentPlayer in Players)
            foreach (var half in statistic.Halves)
            {
                var mvpSiDamage = half.MvpSiDamage;
                var mvpCommon = half.MvpCommon;
                var lvpFfGiven = half.LvpFfGiven;

                foreach (var player in half.Players.Where(w => w.CommunityId == currentPlayer.CommunityId))
                {
                    currentPlayer.Died += player.Died;
                    currentPlayer.Incaps += player.Incaps;
                    currentPlayer.DmgTaken += player.DmgTaken;
                    currentPlayer.Common += player.Common;
                    currentPlayer.SiKilled += player.SiKilled;
                    currentPlayer.SiDamage += player.SiDamage;
                    currentPlayer.TankDamage += player.TankDamage;
                    currentPlayer.RockEats += player.RockEats;
                    currentPlayer.WitchDamage += player.WitchDamage;
                    currentPlayer.Skeets += player.Skeets;
                    currentPlayer.Levels += player.Levels;
                    currentPlayer.Crowns += player.Crowns;
                    currentPlayer.FfGiven += player.FfGiven;

                    if (mvpSiDamage != null && player.CommunityId == mvpSiDamage.CommunityId)
                        currentPlayer.MvpSiDamage++;

                    if (mvpCommon != null && player.CommunityId == mvpCommon.CommunityId)
                        currentPlayer.MvpCommon++;

                    if (lvpFfGiven != null && player.CommunityId == lvpFfGiven.CommunityId)
                        currentPlayer.LvpFfGiven++;
                }

                foreach (var infectedPlayer in half.InfectedPlayers.Where(w => w.CommunityId == currentPlayer.CommunityId))
                {
                    currentPlayer.DmgTotal += infectedPlayer.DmgTotal;
                    currentPlayer.DmgTank += infectedPlayer.DmgTank;
                    currentPlayer.DmgSpit += infectedPlayer.DmgSpit;
                    currentPlayer.HunterDpDmg += infectedPlayer.HunterDpDmg;
                }
            }
        }
    }

    public class Player(PlayerName playerName, Team team)
    {
        public string? SteamId => playerName.SteamId;
        public string? CommunityId => playerName.CommunityId;
        public string? Steam3 => playerName.Steam3;
        public string? ProfileUrl => playerName.ProfileUrl;
        public int Index => playerName.Index;
        public string? Name => playerName.Name;

        /* Survivor */
        public int Died { get; set; }
        public decimal DiedPercentage => SafeDivision(Died, team.Died);
        public int Incaps { get; set; }
        public decimal IncapsPercentage => SafeDivision(Incaps, team.Incaps);
        public int DmgTaken { get; set; }
        public decimal DmgTakenPercentage => SafeDivision(DmgTaken, team.DmgTaken);
        public int Common { get; set; }
        public decimal CommonPercentage => SafeDivision(Common, team.Common);
        public int SiKilled { get; set; }
        public decimal SiKilledPercentage => SafeDivision(SiKilled, team.SiKilled);
        public int SiDamage { get; set; }
        public decimal SiDamagePercentage => SafeDivision(SiDamage, team.SiDamage);
        public int TankDamage { get; set; }
        public decimal TankDamagePercentage => SafeDivision(TankDamage, team.TankDamage);
        public int RockEats { get; set; }
        public decimal RockEatsPercentage => SafeDivision(RockEats, team.RockEats);
        public int WitchDamage { get; set; }
        public decimal WitchDamagePercentage => SafeDivision(WitchDamage, team.WitchDamage);
        public int Skeets { get; set; }
        public decimal SkeetsPercentage => SafeDivision(Skeets, team.Skeets);
        public int Levels { get; set; }
        public decimal LevelsPercentage => SafeDivision(Levels, team.Levels);
        public int Crowns { get; set; }
        public decimal CrownsPercentage => SafeDivision(Crowns, team.Crowns);
        public int FfGiven { get; set; }
        public decimal FfGivenPercentage => SafeDivision(FfGiven, team.FfGiven);

        /* Infected */
        public int DmgTotal { get; set; }
        public decimal DmgTotalPercentage => SafeDivision(DmgTotal, team.DmgTotal);
        public int DmgTank { get; set; }
        public decimal DmgTankPercentage => SafeDivision(DmgTank, team.DmgTank);
        public int DmgSpit { get; set; }
        public decimal DmgSpitPercentage => SafeDivision(DmgSpit, team.DmgSpit);
        public int HunterDpDmg { get; set; }
        public decimal HunterDpDmgPercentage => SafeDivision(HunterDpDmg, team.HunterDpDmg);

        /* MVP and LVP */
        public int MvpSiDamage { get; set; }
        public decimal MvpSiDamagePercentage => SafeDivision(MvpSiDamage, team.MvpSiDamage);
        public int MvpCommon { get; set; }
        public decimal MvpCommonPercentage => SafeDivision(MvpCommon, team.MvpCommon);
        public int LvpFfGiven { get; set; }
        public decimal LvpFfGivenPercentage => SafeDivision(LvpFfGiven, team.LvpFfGiven);

        private static decimal SafeDivision(decimal dividend, decimal divisor)
        {
            if (divisor == 0)
                return 0;

            return dividend / divisor;
        }
    }
}