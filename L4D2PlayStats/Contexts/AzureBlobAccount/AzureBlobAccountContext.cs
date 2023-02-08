using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Microsoft.Extensions.Configuration;

namespace L4D2PlayStats.Contexts.AzureBlobAccount;

public class AzureBlobAccountContext : IAzureBlobAccountContext
{
    private readonly IConfiguration _configuration;

    private readonly Dictionary<string, BlobContainerClient> _containers = new();
    private BlobServiceClient? _blobServiceClient;

    public AzureBlobAccountContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private string ConnectionString => _configuration.GetValue<string>("AzureWebJobsStorage")!;
    private BlobServiceClient BlobServiceClient => _blobServiceClient ??= new BlobServiceClient(ConnectionString);

    public async Task<string> GenerateSasUrlAsync(string blobContainerName, string blobName, BlobSasPermissions permissions, DateTimeOffset expiresOn)
    {
        await CreateContainerIfNotExistsAsync(blobContainerName);

        var blobClient = new BlobBaseClient(ConnectionString, blobContainerName, blobName);
        var uri = blobClient.GenerateSasUri(permissions, expiresOn);
        var publicUrl = uri.ToString();

        return publicUrl;
    }

    public AsyncPageable<BlobContainerItem> GetBlobContainersAsync(string prefix, CancellationToken cancellationToken)
    {
        return BlobServiceClient.GetBlobContainersAsync(BlobContainerTraits.None, prefix, cancellationToken);
    }

    public BlobContainerClient GetBlobContainerClient(string blobContainerName)
    {
        return BlobServiceClient.GetBlobContainerClient(blobContainerName);
    }

    public async Task DeleteBlobContainerAsync(string blobContainerName)
    {
        var blobContainerClient = GetBlobContainerClient(blobContainerName);
        if (!await blobContainerClient.ExistsAsync())
            return;

        await blobContainerClient.DeleteAsync();

        _containers.Remove(blobContainerName);
    }

    private async Task CreateContainerIfNotExistsAsync(string blobContainerName)
    {
        if (_containers.ContainsKey(blobContainerName))
            return;

        var blobContainerClient = BlobServiceClient.GetBlobContainerClient(blobContainerName);
        await blobContainerClient.CreateIfNotExistsAsync();

        _containers.Add(blobContainerName, blobContainerClient);
    }
}