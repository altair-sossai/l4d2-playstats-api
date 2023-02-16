using L4D2PlayStats.Core.Modules.Matches.Results;

namespace L4D2PlayStats.Core.Modules.Matches.Services;

public interface IMatchService
{
	Task<List<MatchResult>> GetMatchesAsync(string server, int statisticsCount);
	Task<List<MatchResult>> GetMatchesBetweenAsync(string server, string start, string end);
}