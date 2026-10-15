using System.Text.Json;
using Azure.Storage.Blobs.Models;
using L4D2PlayStats.Core.Contexts.AzureBlobStorage;
using L4D2PlayStats.Core.Modules.Matches.Services;
using L4D2PlayStats.Core.Modules.Stars.Extensions;
using L4D2PlayStats.Core.Modules.Statistics.Models;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Core.Modules.Stars.Services;

public class StarsService(
    IMemoryCache memoryCache,
    IMatchService matchService,
    IAzureBlobStorageContext blobStorageContext) : IStarsService
{
    public async Task<List<PlayerStars>> StarsAsync(string serverId)
    {
        var closed = await ClosedStarsAsync(serverId);
        var current = await CurrentStarsAsync(serverId);

        var players = closed.ToDictionary(k => k.CommunityId, v => new PlayerStars
        {
            CommunityId = v.CommunityId,
            MvpSiDamage = v.MvpSiDamage,
            MvpCommon = v.MvpCommon
        });

        foreach (var (communityId, stars) in current)
        {
            if (!players.TryGetValue(communityId, out var player))
            {
                player = new PlayerStars { CommunityId = communityId };
                players.Add(communityId, player);
            }

            player.MvpSiDamage += stars.MvpSiDamage;
            player.MvpCommon += stars.MvpCommon;
        }

        return
        [
            .. players.Values
                .OrderByDescending(o => o.Stars)
                .ThenBy(o => o.CommunityId)
        ];
    }

    public async Task<PlayerStars> StarsAsync(string serverId, long communityId)
    {
        var players = await StarsAsync(serverId);

        return players.FirstOrDefault(f => f.CommunityId == communityId) ?? new PlayerStars { CommunityId = communityId };
    }

    public async Task SaveClosedStarsAsync(string serverId)
    {
        await ClosedStarsAsync(serverId);
    }

    private Task<Dictionary<long, PlayerStars>> CurrentStarsAsync(string serverId)
    {
        return memoryCache.GetOrCreateAsync($"stars_{serverId}".ToLower(), async factory =>
        {
            factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

            var matches = await matchService.GetMatchesAsync(serverId);

            return matches.Stars();
        })!;
    }

    private Task<List<PlayerStars>> ClosedStarsAsync(string serverId)
    {
        var period = new RankingPeriodModel(DateTime.UtcNow);
        var fileName = $"closed_{period.Start:yyyyMM}.json";

        return memoryCache.GetOrCreateAsync($"stars_closed_{serverId}_{fileName}".ToLower(), async factory =>
        {
            factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12);

            var containerName = $"{serverId}-stars".ToLower();

            await blobStorageContext.CreateContainerIfNotExistsAsync(containerName, PublicAccessType.None);

            var blobClient = blobStorageContext.GetBlobClient(containerName, fileName);

            if (await blobClient.ExistsAsync())
            {
                var result = await blobClient.DownloadContentAsync();

                return JsonSerializer.Deserialize<List<PlayerStars>>(result.Value.Content.ToString())!;
            }

            var matches = await matchService.GetMatchesBeforeAsync(serverId, period.Start);
            var players = matches.Stars().Values.OrderBy(o => o.CommunityId).ToList();

            await blobStorageContext.UploadAsync(containerName, fileName, players);

            return players;
        })!;
    }
}