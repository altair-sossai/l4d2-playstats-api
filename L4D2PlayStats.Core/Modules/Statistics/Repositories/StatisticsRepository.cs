using L4D2PlayStats.Core.Contexts.AzureTableStorage;
using L4D2PlayStats.Core.Contexts.AzureTableStorage.Repositories;

namespace L4D2PlayStats.Core.Modules.Statistics.Repositories;

public class StatisticsRepository : BaseTableStorageRepository<Statistics>, IStatisticsRepository
{
    public StatisticsRepository(IAzureTableStorageContext tableContext)
        : base("Statistics", tableContext)
    {
    }

    public ValueTask<Statistics?> GetStatisticAsync(string server, string fileName)
    {
        return FindAsync(server, fileName);
    }

    public IAsyncEnumerable<Statistics> GetStatisticsAsync(string server)
    {
        return GetAllAsync(server);
    }
}