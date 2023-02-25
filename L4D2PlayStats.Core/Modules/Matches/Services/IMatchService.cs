namespace L4D2PlayStats.Core.Modules.Matches.Services;

public interface IMatchService
{
    Task<Match?> LastMatchAsync(string server);
    Task<List<Match>> GetMatchesAsync(string server);
    Task<List<Match>> GetMatchesBetweenAsync(string server, string start, string end);
}