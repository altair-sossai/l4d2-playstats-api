using System.Text;
using System.Text.Json;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace L4D2PlayStats.Core.Contexts.AzureBlobStorage;

public class AzureBlobStorageContext(IConfiguration configuration) : IAzureBlobStorageContext
{
    private BlobServiceClient? _blobServiceClient;

    private string ConnectionString => configuration.GetValue<string>("AzureWebJobsStorage")!;
    private BlobServiceClient BlobServiceClient => _blobServiceClient ??= new BlobServiceClient(ConnectionString);

    public BlobClient GetBlobClient(string containerName, string fileName)
    {
        var containerClient = BlobServiceClient.GetBlobContainerClient(containerName);

        return containerClient.GetBlobClient(fileName);
    }

    public AsyncPageable<BlobItem> GetBlobsAsync(string containerName)
    {
        var containerClient = BlobServiceClient.GetBlobContainerClient(containerName);

        return containerClient.GetBlobsAsync();
    }

    public async Task CreateContainerIfNotExistsAsync(string containerName, PublicAccessType accessType)
    {
        var containerClient = BlobServiceClient.GetBlobContainerClient(containerName);

        await containerClient.CreateIfNotExistsAsync(accessType);
    }

    public async Task UploadAsync<T>(string containerName, string fileName, T t)
    {
        var containerClient = BlobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(fileName);

        var json = JsonSerializer.Serialize(t);
        var bytes = Encoding.UTF8.GetBytes(json);

        using var stream = new MemoryStream(bytes);

        await blobClient.UploadAsync(stream, true);
    }
}