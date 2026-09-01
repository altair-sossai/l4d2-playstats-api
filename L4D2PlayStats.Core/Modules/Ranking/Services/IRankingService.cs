using L4D2PlayStats.Core.Modules.Ranking.Model;

namespace L4D2PlayStats.Core.Modules.Ranking.Services;

public interface IRankingService
{
    Task<List<Player>> RankingAsync(string serverId, int count, DateTime? reference = null);
    Task<bool> SaveRankingAsync(string serverId, DateTime reference);
    Task SaveAnnualRankingAsync(string serverId, int year);
    Task SaveAllTimeRankingAsync(string serverId);
    IAsyncEnumerable<HistoryModel> AllHistoryAsync(string serverId);
    Task<List<Player>> HistoryAsync(string serverId, string historyId);
}