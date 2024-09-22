using L4D2PlayStats.Core.Contexts.AzureTableStorage;
using L4D2PlayStats.Core.Contexts.AzureTableStorage.Repositories;
using L4D2PlayStats.Core.Modules.Statistics.Helpers;

namespace L4D2PlayStats.Core.Modules.Statistics.Repositories;

public class StatisticsRepository(IAzureTableStorageContext tableContext) : BaseTableStorageRepository<Statistics>("Statistics", tableContext), IStatisticsRepository
{
    public ValueTask<Statistics?> GetStatisticAsync(string serverId, string statisticId)
    {
        return FindAsync(serverId, statisticId);
    }

    public IAsyncEnumerable<Statistics> GetStatisticsAsync(string serverId)
    {
        var rankingPeriod = StatisticsHelper.CurrentRankingPeriod(DateTime.UtcNow);
        var rowKey = $"{long.MaxValue - rankingPeriod.Ticks}";
        var filter = $"PartitionKey eq '{serverId}' and RowKey le '{rowKey}'";

        return TableClient.QueryAsync<Statistics>(filter);
    }

    public IAsyncEnumerable<Statistics> GetStatisticsBetweenAsync(string serverId, string start, string end)
    {
        var filter = $"PartitionKey eq '{serverId}' and RowKey ge '{start}' and RowKey le '{end}'";

        return TableClient.QueryAsync<Statistics>(filter);
    }
}