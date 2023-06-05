using System.Runtime.Serialization;
using L4D2PlayStats.Core.Modules.Campaigns;

namespace L4D2PlayStats.Core.Modules.Matches;

public class Match
{
    public Match(Campaign campaign, Scoring.Team teamA, List<PlayerName> playersA, Scoring.Team teamB, List<PlayerName> playersB)
    {
        Campaign = campaign.Name;

        Teams = new List<Team>
        {
            new(teamA, playersA), new(teamB, playersB)
        };
    }

    public int TeamSize { get; internal init; }
    public DateTime MatchStart { get; internal set; }
    public DateTime? MatchEnd { get; internal init; }
    public TimeSpan? MatchElapsed => MatchEnd - MatchStart;
    public string? Campaign { get; }

    public List<Team> Teams { get; }
    public List<string> Statistics { get; } = new();

    [IgnoreDataMember]
    public List<Statistics.Statistics> Maps { get; } = new();

    public bool Competitive => Maps.Count >= 4 && TeamSize == 4;

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

        public Team(Scoring.Team team, IEnumerable<PlayerName> players)
        {
            _team = team;
            Players = players.Select(playerName => new Player(playerName, this)).ToList();
        }

        public int Score => _team.Score;
        public List<Player> Players { get; }
        public int Common => Players.Select(p => p.Common).DefaultIfEmpty(0).Sum();
        public int SiKilled => Players.Select(p => p.SiKilled).DefaultIfEmpty(0).Sum();
        public int SiDamage => Players.Select(p => p.SiDamage).DefaultIfEmpty(0).Sum();
        public int DmgTotal => Players.Select(p => p.DmgTotal).DefaultIfEmpty(0).Sum();
        public int DmgTank => Players.Select(p => p.DmgTank).DefaultIfEmpty(0).Sum();

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
                foreach (var player in half.Players.Where(w => w.CommunityId == currentPlayer.CommunityId))
                {
                    currentPlayer.Common += player.Common;
                    currentPlayer.SiKilled += player.SiKilled;
                    currentPlayer.SiDamage += player.SiDamage;
                }

                foreach (var infectedPlayer in half.InfectedPlayers.Where(w => w.CommunityId == currentPlayer.CommunityId))
                {
                    currentPlayer.DmgTotal += infectedPlayer.DmgTotal;
                    currentPlayer.DmgTank += infectedPlayer.DmgTank;
                }
            }
        }
    }

    public class Player
    {
        private readonly PlayerName _playerName;
        private readonly Team _team;

        public Player(PlayerName playerName, Team team)
        {
            _playerName = playerName;
            _team = team;
        }

        public string? SteamId => _playerName.SteamId;
        public string? CommunityId => _playerName.CommunityId;
        public string? Steam3 => _playerName.Steam3;
        public string? ProfileUrl => _playerName.ProfileUrl;
        public int Index => _playerName.Index;
        public string? Name => _playerName.Name;

        /* Survivor */
        public int Common { get; set; }
        public decimal CommonPercentage => SafeDivision(Common, _team.Common);
        public int SiKilled { get; set; }
        public decimal SiKilledPercentage => SafeDivision(SiKilled, _team.SiKilled);
        public int SiDamage { get; set; }
        public decimal SiDamagePercentage => SafeDivision(SiDamage, _team.SiDamage);

        /* Infected */
        public int DmgTotal { get; set; }
        public decimal DmgTotalPercentage => SafeDivision(DmgTotal, _team.DmgTotal);
        public int DmgTank { get; set; }
        public decimal DmgTankPercentage => SafeDivision(DmgTank, _team.DmgTank);

        private static decimal SafeDivision(decimal dividend, decimal divisor)
        {
            if (divisor == 0)
                return 0;

            return dividend / divisor;
        }
    }
}