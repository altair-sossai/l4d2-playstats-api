using L4D2PlayStats.Core.Contexts.AzureTableStorage;
using L4D2PlayStats.Core.Contexts.AzureTableStorage.Repositories;
using L4D2PlayStats.Core.Modules.Ranking.Configs;

namespace L4D2PlayStats.Core.Modules.Statistics.Repositories;

public class StatisticsRepository(
    IAzureTableStorageContext tableContext,
    IExperienceConfig config)
    : BaseTableStorageRepository<Statistics>("Statistics", tableContext), IStatisticsRepository
{
    public ValueTask<Statistics?> GetStatisticAsync(string serverId, string statisticId)
    {
        return FindAsync(serverId, statisticId);
    }

    public IAsyncEnumerable<Statistics> GetStatisticsAsync(string serverId)
    {
        var after = DateTime.UtcNow.AddDays(config.DaysRange * -1);
        var rowKey = $"{long.MaxValue - after.Ticks}";
        var filter = $"PartitionKey eq '{serverId}' and RowKey le '{rowKey}'";

        return TableClient.QueryAsync<Statistics>(filter);
    }

    public IAsyncEnumerable<Statistics> GetStatisticsBetweenAsync(string serverId, string start, string end)
    {
        var filter = $"PartitionKey eq '{serverId}' and RowKey ge '{start}' and RowKey le '{end}'";

        return TableClient.QueryAsync<Statistics>(filter);
    }
}