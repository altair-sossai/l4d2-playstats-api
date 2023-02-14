namespace L4D2PlayStats.Core.Modules.Statistics.Repositories;

public interface IStatisticsRepository
{
	ValueTask<Statistics?> GetStatisticAsync(string server, string fileName);
	IAsyncEnumerable<Statistics> GetStatisticsAsync(string server);
	Task AddOrUpdateAsync(Statistics statistics);
}