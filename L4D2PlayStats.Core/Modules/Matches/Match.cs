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

    public DateTime MatchStart { get; internal set; }
    public DateTime? MatchEnd { get; internal init; }
    public TimeSpan? MatchElapsed => MatchEnd - MatchStart;
    public string? Campaign { get; }

    public List<Team> Teams { get; }
    public List<string> Statistics { get; } = new();

    [IgnoreDataMember]
    public List<Statistics.Statistics> Maps { get; } = new();

    public void Add(Statistics.Statistics statistic)
    {
        Statistics.Add(statistic.RowKey);
        Maps.Add(statistic);
    }

    public class Team
    {
        private readonly Scoring.Team _team;

        public Team(Scoring.Team team, List<PlayerName> players)
        {
            _team = team;
            Players = players;
        }

        public int Score => _team.Score;
        public List<PlayerName> Players { get; }
    }
}