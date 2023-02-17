﻿using L4D2PlayStats.Core.Contexts.AzureTableStorage;
using L4D2PlayStats.Core.Contexts.AzureTableStorage.Repositories;

namespace L4D2PlayStats.Core.Modules.Statistics.Repositories;

public class StatisticsRepository : BaseTableStorageRepository<Statistics>, IStatisticsRepository
{
    public StatisticsRepository(IAzureTableStorageContext tableContext)
        : base("Statistics", tableContext)
    {
    }

    public ValueTask<Statistics?> GetStatisticAsync(string server, string statisticId)
    {
        return FindAsync(server, statisticId);
    }

    public IAsyncEnumerable<Statistics> GetStatisticsAsync(string server)
    {
        return GetAllAsync(server);
    }

    public IAsyncEnumerable<Statistics> GetStatisticsBetweenAsync(string server, string start, string end)
    {
        var filter = $@"PartitionKey eq '{server}' and RowKey ge '{start}' and RowKey le '{end}'";

        return TableClient.QueryAsync<Statistics>(filter);
    }
}