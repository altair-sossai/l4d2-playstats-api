using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

namespace L4D2PlayStats.Contexts.AzureBlobAccount;

public interface IAzureBlobAccountContext
{
    Task<string> GenerateSasUrlAsync(string blobContainerName, string blobName, BlobSasPermissions permissions, DateTimeOffset expiresOn);
    AsyncPageable<BlobContainerItem> GetBlobContainersAsync(string prefix, CancellationToken cancellationToken);
    BlobContainerClient GetBlobContainerClient(string blobContainerName);
    Task DeleteBlobContainerAsync(string blobContainerName);
}