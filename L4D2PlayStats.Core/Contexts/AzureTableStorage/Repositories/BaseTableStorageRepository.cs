using Azure.Data.Tables;

namespace L4D2PlayStats.Core.Contexts.AzureTableStorage.Repositories;

public abstract class BaseTableStorageRepository
{
    private static readonly HashSet<string> CreatedTables = [];

    private readonly IAzureTableStorageContext _tableContext;
    private readonly string _tableName;

    private TableClient? _tableClient;

    protected BaseTableStorageRepository(string tableName,
        IAzureTableStorageContext tableContext)
    {
        _tableName = tableName;
        _tableContext = tableContext;

        CreateIfNotExistsAsync().Wait();
    }

    protected TableClient TableClient => _tableClient ??= _tableContext.GetTableClientAsync(_tableName).Result;

    private async Task CreateIfNotExistsAsync()
    {
        if (CreatedTables.Contains(_tableName))
            return;

        await TableClient.CreateIfNotExistsAsync();

        CreatedTables.Add(_tableName);
    }
}

public abstract class BaseTableStorageRepository<TEntity>(string tableName, IAzureTableStorageContext tableContext)
    : BaseTableStorageRepository(tableName, tableContext)
    where TEntity : class, ITableEntity, new()
{
    public ValueTask<TEntity?> FindAsync(string partitionKey, string rowKey)
    {
        return TableClient.QueryAsync<TEntity>(q => q.PartitionKey == partitionKey && q.RowKey == rowKey).FirstOrDefaultAsync();
    }

    public virtual Task AddOrUpdateAsync(TEntity entity)
    {
        return TableClient.UpsertEntityAsync(entity);
    }
}