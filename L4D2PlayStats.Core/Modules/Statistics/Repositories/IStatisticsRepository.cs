namespace L4D2PlayStats.Core.Modules.Statistics.Repositories;

public interface IStatisticsRepository
{
    Statistics? GetStatistic(string server, string fileName);
    IEnumerable<Statistics> GetStatistics(string server);
    Task AddOrUpdateAsync(Statistics statistics);
}