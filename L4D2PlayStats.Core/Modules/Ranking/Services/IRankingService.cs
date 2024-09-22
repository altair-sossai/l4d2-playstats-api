using L4D2PlayStats.Core.Modules.Ranking.Model;

namespace L4D2PlayStats.Core.Modules.Ranking.Services;

public interface IRankingService
{
    Task<List<Player>> RankingAsync(string serverId, int count, DateTime? reference = null);
    Task SaveRankingAsync(string serverId, DateTime reference);
    IAsyncEnumerable<HistoryModel> AllHistoryAsync(string serverId);
    Task<List<Player>> HistoryAsync(string serverId, string historyId);
}