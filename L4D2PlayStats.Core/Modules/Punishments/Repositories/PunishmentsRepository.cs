using L4D2PlayStats.Core.Contexts.AzureTableStorage;
using L4D2PlayStats.Core.Contexts.AzureTableStorage.Repositories;

namespace L4D2PlayStats.Core.Modules.Punishments.Repositories;

public class PunishmentsRepository(IAzureTableStorageContext tableContext) : BaseTableStorageRepository<Punishment>("Punishments", tableContext), IPunishmentsRepository
{
    public IAsyncEnumerable<Punishment> GetPunishmentsAsync(string serverId)
    {
        var filter = $"PartitionKey eq '{serverId}'";

        return TableClient.QueryAsync<Punishment>(filter);
    }

    public Task DeleteAsync(string serverId, string communityId)
    {
        return TableClient.DeleteEntityAsync(serverId, communityId);
    }

    public async Task DeleteAllAsync(string serverId)
    {
        await foreach (var punishment in GetPunishmentsAsync(serverId))
            await TableClient.DeleteEntityAsync(punishment);
    }
}