namespace L4D2PlayStats.Core.Modules.Statistics.Repositories;

public interface IStatisticsRepository
{
	ValueTask<Statistics?> GetStatisticAsync(string server, string statisticId);
	IAsyncEnumerable<Statistics> GetStatisticsAsync(string server);
	IAsyncEnumerable<Statistics> GetStatisticsBetweenAsync(string server, string start, string end);
	Task AddOrUpdateAsync(Statistics statistics);
}