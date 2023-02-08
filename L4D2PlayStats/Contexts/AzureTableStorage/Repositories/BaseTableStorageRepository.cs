using Azure.Data.Tables;

namespace L4D2PlayStats.Contexts.AzureTableStorage.Repositories;

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

        CreateIfNotExists();
    }

    protected TableClient TableClient => _tableClient ??= _tableContext.GetTableClient(_tableName).Result;

    protected void Delete(string partitionKey, string rowKey)
    {
        TableClient.DeleteEntity(partitionKey, rowKey);
    }

    private void CreateIfNotExists()
    {
        if (CreatedTables.Contains(_tableName))
            return;

        TableClient.CreateIfNotExists();

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

    public virtual void Add(TEntity entity)
    {
        TableClient.AddEntity(entity);
    }

    public virtual void AddOrUpdate(TEntity entity)
    {
        TableClient.UpsertEntity(entity);
    }

    public void AddOrUpdate(IEnumerable<TEntity> entities)
    {
        var transaction = entities
            .Select(entity => new TableTransactionAction(TableTransactionActionType.UpsertMerge, entity))
            .ToList();

        TableClient.SubmitTransaction(transaction);
    }
}