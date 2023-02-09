using Azure.Data.Tables;

namespace L4D2PlayStats.Core.Contexts.AzureTableStorage.Repositories;

public abstract class BaseTableStorageRepository
{
    private static readonly HashSet<string> CreatedTables = new();

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

    protected async Task DeleteAsync(string partitionKey, string rowKey)
    {
        await TableClient.DeleteEntityAsync(partitionKey, rowKey);
    }

    private async Task CreateIfNotExistsAsync()
    {
        if (CreatedTables.Contains(_tableName))
            return;

        await TableClient.CreateIfNotExistsAsync();

        CreatedTables.Add(_tableName);
    }
}

public abstract class BaseTableStorageRepository<TEntity> : BaseTableStorageRepository
    where TEntity : class, ITableEntity, new()
{
    protected BaseTableStorageRepository(string tableName, IAzureTableStorageContext tableContext)
        : base(tableName, tableContext)
    {
    }

    protected bool Exists(string partitionKey, string rowKey)
    {
        return TableClient.Query<TEntity>(q => q.PartitionKey == partitionKey && q.RowKey == rowKey).Any();
    }

    protected TEntity? Find(string partitionKey, string rowKey)
    {
        return TableClient.Query<TEntity>(q => q.PartitionKey == partitionKey && q.RowKey == rowKey).FirstOrDefault();
    }

    protected IEnumerable<TEntity> GetAll()
    {
        return TableClient.Query<TEntity>();
    }

    protected IEnumerable<TEntity> GetAll(string partitionKey)
    {
        return TableClient.Query<TEntity>(q => q.PartitionKey == partitionKey);
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await TableClient.AddEntityAsync(entity);
    }

    public virtual async Task AddOrUpdateAsync(TEntity entity)
    {
        await TableClient.UpsertEntityAsync(entity);
    }

    public async Task AddOrUpdateAsync(IEnumerable<TEntity> entities)
    {
        var transaction = entities
            .Select(entity => new TableTransactionAction(TableTransactionActionType.UpsertMerge, entity))
            .ToList();

        await TableClient.SubmitTransactionAsync(transaction);
    }
}