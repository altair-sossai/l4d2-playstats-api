namespace L4D2PlayStats.Core.Modules.Matches.Services;

public interface IMatchService
{
    Task<Match?> LastMatchAsync(string serverId);
    Task<List<Match>> GetMatchesAsync(string serverId, DateTime? reference = null, bool competitiveOnly = true);
    Task<List<Match>> GetMatchesAsync(string serverId, DateTime start, DateTime end, bool competitiveOnly = true);
    Task<List<Match>> GetMatchesAsync(string serverId, string start, string end, bool competitiveOnly = true);
    Task<List<Match>> GetAllMatchesAsync(string serverId, bool competitiveOnly = true);
}