using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;

namespace L4D2PlayStats.Core.Contexts.AzureTableStorage;

public class AzureTableStorageContext(IConfiguration configuration)
    : IAzureTableStorageContext
{
    private static readonly HashSet<string> CreatedTables = [];
    private TableServiceClient? _tableServiceClient;

    private string ConnectionString => configuration.GetValue<string>("AzureWebJobsStorage")!;
    private TableServiceClient TableServiceClient => _tableServiceClient ??= new TableServiceClient(ConnectionString);

    public async Task<TableClient> GetTableClientAsync(string tableName)
    {
        var tableClient = TableServiceClient.GetTableClient(tableName);

        if (CreatedTables.Contains(tableName))
            return tableClient;

        await tableClient.CreateIfNotExistsAsync();
        CreatedTables.Add(tableName);

        return tableClient;
    }
}