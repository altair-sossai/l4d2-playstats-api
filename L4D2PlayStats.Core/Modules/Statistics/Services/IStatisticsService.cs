using L4D2PlayStats.Core.Modules.Statistics.Commands;

namespace L4D2PlayStats.Core.Modules.Statistics.Services;

public interface IStatisticsService
{
	Task<Statistics> AddOrUpdateAsync(string server, StatisticsCommand command);
}