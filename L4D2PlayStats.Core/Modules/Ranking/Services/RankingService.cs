using Azure.Storage.Blobs.Models;
using L4D2PlayStats.Core.Contexts.AzureBlobStorage;
using L4D2PlayStats.Core.Modules.Matches.Services;
using L4D2PlayStats.Core.Modules.Ranking.Configs;
using L4D2PlayStats.Core.Modules.Ranking.Extensions;
using L4D2PlayStats.Core.Modules.Statistics.Models;

namespace L4D2PlayStats.Core.Modules.Ranking.Services;

public class RankingService(IMatchService matchService, IExperienceConfig config, IAzureBlobStorageContext blobStorageContext) : IRankingService
{
    public async Task<List<Player>> RankingAsync(string serverId, int count, DateTime? reference = null)
    {
        var matches = await matchService.GetMatchesAsync(serverId, reference);
        var players = matches.Ranking(config).Take(count).ToList();

        return players;
    }

    public async Task SaveRankingAsync(string serverId, DateTime reference)
    {
        var containerName = $"{serverId}-ranking-history".ToLower();

        await blobStorageContext.CreateContainerIfNotExistsAsync(containerName, PublicAccessType.Blob);

        var period = new RankingPeriodModel(reference);
        var fileName = $"ranking_{period.Start:yyyy-MM}_{period.End:yyyy-MM}.json".ToLower();

        var blobClient = blobStorageContext.GetBlobClient(containerName, fileName);

        if (await blobClient.ExistsAsync())
            return;

        var players = await RankingAsync(serverId, 100, reference);

        await blobStorageContext.UploadAsync(containerName, fileName, players);
    }
}