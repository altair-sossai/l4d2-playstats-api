using System.Text.Json;
using Azure.Storage.Blobs.Models;
using L4D2PlayStats.Core.Contexts.AzureBlobStorage;
using L4D2PlayStats.Core.Modules.Matches.Services;
using L4D2PlayStats.Core.Modules.Ranking.Configs;
using L4D2PlayStats.Core.Modules.Ranking.Extensions;
using L4D2PlayStats.Core.Modules.Ranking.Model;
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
        var fileName = $"ranking_{period.Start:yyyyMM}{period.End:yyyyMM}.json".ToLower();

        var blobClient = blobStorageContext.GetBlobClient(containerName, fileName);

        if (await blobClient.ExistsAsync())
            return;

        var players = await RankingAsync(serverId, 100, reference);

        await blobStorageContext.UploadAsync(containerName, fileName, players);
    }

    public async IAsyncEnumerable<HistoryModel> AllHistoryAsync(string serverId)
    {
        var containerName = $"{serverId}-ranking-history".ToLower();

        await foreach (var blobItem in blobStorageContext.GetBlobsAsync(containerName))
        {
            var history = HistoryModel.Parse(blobItem.Name);

            if (history != null)
                yield return new HistoryModel(blobItem.Name);
        }
    }

    public async Task<List<Player>> HistoryAsync(string serverId, string historyId)
    {
        var containerName = $"{serverId}-ranking-history".ToLower();
        var fileName = $"ranking_{historyId}.json".ToLower();

        var blobClient = blobStorageContext.GetBlobClient(containerName, fileName);

        if (!await blobClient.ExistsAsync())
            return [];

        var result = await blobClient.DownloadContentAsync();
        var json = result.Value.Content.ToString();

        return JsonSerializer.Deserialize<List<Player>>(json)!;
    }
}