namespace L4D2PlayStats.Core.Modules.Statistics.Repositories;

public interface IStatisticsRepository
{
    ValueTask<Statistics?> GetStatisticAsync(string serverId, string statisticId);
    IAsyncEnumerable<Statistics> GetStatisticsAsync(string serverId, DateTime? reference = null);
    IAsyncEnumerable<Statistics> GetStatisticsAsync(string serverId, DateTime start, DateTime end);
    IAsyncEnumerable<Statistics> GetStatisticsBetweenAsync(string serverId, string start, string end);
    Task AddOrUpdateAsync(Statistics statistics);
}