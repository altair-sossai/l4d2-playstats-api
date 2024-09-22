namespace L4D2PlayStats.Core.Modules.Matches.Services;

public interface IMatchService
{
    Task<Match?> LastMatchAsync(string serverId);
    Task<List<Match>> GetMatchesAsync(string serverId, DateTime? reference = null);
    Task<List<Match>> GetMatchesBetweenAsync(string serverId, string start, string end);
}