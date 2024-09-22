using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace L4D2PlayStats.Core.Contexts.AzureBlobStorage;

public interface IAzureBlobStorageContext
{
    BlobClient GetBlobClient(string containerName, string fileName);
    Task CreateContainerIfNotExistsAsync(string containerName, PublicAccessType accessType);
    Task UploadAsync<T>(string containerName, string fileName, T t);
}