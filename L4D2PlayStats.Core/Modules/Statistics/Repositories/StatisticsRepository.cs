using L4D2PlayStats.Core.Contexts.AzureTableStorage;
using L4D2PlayStats.Core.Contexts.AzureTableStorage.Repositories;
using L4D2PlayStats.Core.Modules.Statistics.Models;

namespace L4D2PlayStats.Core.Modules.Statistics.Repositories;

public class StatisticsRepository(IAzureTableStorageContext tableContext) : BaseTableStorageRepository<Statistics>("Statistics", tableContext), IStatisticsRepository
{
    public ValueTask<Statistics?> GetStatisticAsync(string serverId, string statisticId)
    {
        return FindAsync(serverId, statisticId);
    }

    public IAsyncEnumerable<Statistics> GetStatisticsAsync(string serverId, DateTime? reference = null)
    {
        var period = new RankingPeriodModel(reference ?? DateTime.UtcNow);
        var start = $"{long.MaxValue - period.End.Ticks}";
        var end = $"{long.MaxValue - period.Start.Ticks}";

        return GetStatisticsBetweenAsync(serverId, start, end);
    }

    public IAsyncEnumerable<Statistics> GetStatisticsBetweenAsync(string serverId, string start, string end)
    {
        var filter = $"PartitionKey eq '{serverId}' and RowKey ge '{start}' and RowKey le '{end}'";

        return TableClient.QueryAsync<Statistics>(filter);
    }
}