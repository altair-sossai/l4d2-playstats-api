namespace L4D2PlayStats.Core.Modules.Matches.Services;

public interface IMatchService
{
    Task<List<Match>> GetMatchesAsync(string server, int statisticsCount);
    Task<List<Match>> GetMatchesAsync(string server, DateTime after);
    Task<List<Match>> GetMatchesBetweenAsync(string server, string start, string end);
}