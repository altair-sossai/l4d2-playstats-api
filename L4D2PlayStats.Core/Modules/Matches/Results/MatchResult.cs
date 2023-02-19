using L4D2PlayStats.Core.Modules.Campaigns;

namespace L4D2PlayStats.Core.Modules.Matches.Results;

public class MatchResult
{
    public MatchResult(Campaign campaign, Scoring.Team teamA, List<PlayerName> playersA, Scoring.Team teamB, List<PlayerName> playersB)
    {
        Campaign = campaign.Name;

        Teams = new List<TeamResult>
        {
            new(teamA, playersA), new(teamB, playersB)
        };
    }

    public DateTime MatchStart { get; internal set; }
    public DateTime? MatchEnd { get; internal init; }
    public TimeSpan? MatchElapsed => MatchEnd - MatchStart;
    public string? Campaign { get; }

    public List<TeamResult> Teams { get; }
    public List<string> Statistics { get; } = new();

    public class TeamResult
    {
        private readonly Scoring.Team _team;

        public TeamResult(Scoring.Team team, List<PlayerName> players)
        {
            _team = team;
            Players = players;
        }

        public int Score => _team.Score;
        public List<PlayerName> Players { get; }
    }
}