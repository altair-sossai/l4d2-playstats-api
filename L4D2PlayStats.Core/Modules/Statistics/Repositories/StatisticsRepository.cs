using L4D2PlayStats.Core.Contexts.AzureTableStorage;
using L4D2PlayStats.Core.Contexts.AzureTableStorage.Repositories;

namespace L4D2PlayStats.Core.Modules.Statistics.Repositories;

public class StatisticsRepository : BaseTableStorageRepository<Statistics>, IStatisticsRepository
{
    public StatisticsRepository(IAzureTableStorageContext tableContext)
        : base("Statistics", tableContext)
    {
    }

    public Statistics? GetStatistic(string server, string fileName)
    {
        return Find(server, fileName);
    }

    public IEnumerable<Statistics> GetStatistics(string server)
    {
        return GetAll(server).OrderByDescending(o => o.FileName);
    }
}