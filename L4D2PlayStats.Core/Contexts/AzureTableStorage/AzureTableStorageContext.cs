using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace L4D2PlayStats.Core.Contexts.AzureTableStorage;

public class AzureTableStorageContext(IConfiguration configuration)
    : IAzureTableStorageContext
{
    private static readonly HashSet<string> CreatedTables = [];
    private BlobServiceClient? _blobServiceClient;
    private TableServiceClient? _tableServiceClient;

    private string ConnectionString => configuration.GetValue<string>("AzureWebJobsStorage")!;
    private TableServiceClient TableServiceClient => _tableServiceClient ??= new TableServiceClient(ConnectionString);
    private BlobServiceClient BlobServiceClient => _blobServiceClient ??= new BlobServiceClient(ConnectionString);

    public async Task<TableClient> GetTableClientAsync(string tableName)
    {
        var tableClient = TableServiceClient.GetTableClient(tableName);

        if (CreatedTables.Contains(tableName))
            return tableClient;

        await tableClient.CreateIfNotExistsAsync();
        CreatedTables.Add(tableName);

        return tableClient;
    }

    public Task<Response<BlobContentInfo>> UploadHtmlFileToBlobAsync(string containerName, string blobName, Stream content)
    {
        var blobContainerClient = BlobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = blobContainerClient.GetBlobClient(blobName);

        var blobUploadOptions = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders { ContentType = "text/html" },
            Conditions = null
        };

        return blobClient.UploadAsync(content, blobUploadOptions);
    }
}