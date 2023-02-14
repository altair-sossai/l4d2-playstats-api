using L4D2PlayStats.Core.Modules.Campaigns;

namespace L4D2PlayStats.Core.Modules.Matches.Results;

public class MatchResult
{
	public MatchResult(GameRound gameRound, Campaign campaign)
	{
		MatchDate = gameRound.When;
		Campaign = campaign.Name;
	}

	public DateTime MatchDate { get; }
	public string? Campaign { get; }

	public List<TeamResult> Teams { get; private set; } = new();
	public List<string> Statistics { get; } = new();

	public string? FirstStatistic => Statistics.FirstOrDefault();
	public string? LastStatistic => Statistics.FirstOrDefault();

	public void Update(string statistic, Scoring.Team teamA, List<PlayerName> playersA, Scoring.Team teamB, List<PlayerName> playersB)
	{
		Statistics.Add(statistic);
		Teams = new List<TeamResult>
		{
			new(teamA, playersA), new(teamB, playersB)
		};
	}

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