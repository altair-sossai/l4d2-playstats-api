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

	protected ValueTask<TEntity?> FindAsync(string partitionKey, string rowKey)
	{
		return TableClient.QueryAsync<TEntity>(q => q.PartitionKey == partitionKey && q.RowKey == rowKey).FirstOrDefaultAsync();
	}

	protected IAsyncEnumerable<TEntity> GetAllAsync(string partitionKey)
	{
		return TableClient.QueryAsync<TEntity>(q => q.PartitionKey == partitionKey);
	}

	public virtual Task AddOrUpdateAsync(TEntity entity)
	{
		return TableClient.UpsertEntityAsync(entity);
	}
}