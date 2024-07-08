using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs.Models;

namespace L4D2PlayStats.Core.Contexts.AzureTableStorage;

public interface IAzureTableStorageContext
{
    Task<TableClient> GetTableClientAsync(string tableName);
    Task<Response<BlobContentInfo>> UploadFileToBlobAsync(string containerName, string blobName, Stream content);
}