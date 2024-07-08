using L4D2PlayStats.Core.Contexts.AzureTableStorage;
using L4D2PlayStats.Core.Contexts.Steam;
using L4D2PlayStats.Core.Infrastructure.Helpers;
using L4D2PlayStats.Core.Modules.Matches.Services;
using L4D2PlayStats.Core.Modules.Ranking.Extensions;
using L4D2PlayStats.Core.Modules.Ranking.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Stubble.Core.Builders;

namespace L4D2PlayStats.Core.Modules.Ranking.Services;

public class RankingService(
    IMemoryCache memoryCache,
    IMatchService matchService,
    IAzureTableStorageContext azureTableStorageContext,
    ISteamContext steamContext,
    IConfiguration configuration) : IRankingService
{
    private static string? _rankingTemplate;

    public Task<List<Player>> RankingAsync(string serverId)
    {
        return memoryCache.GetOrCreateAsync($"ranking_{serverId}".ToLower(), async factory =>
        {
            factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

            var matches = await matchService.GetMatchesAsync(serverId);
            var players = matches.Ranking().ToList();

            await UpdateRankingPageAsync(players);

            return players;
        })!;
    }

    private async Task UpdateRankingPageAsync(List<Player> players)
    {
        if (players.Count == 0)
            return;

        var rankingPage = new RankingPageModel(players);
        var steamUserService = steamContext.SteamUserService;
        var steamApiKey = configuration.GetValue<string>("SteamApiKey")!;

        try
        {
            var steamIds = string.Join(',', rankingPage.Players.Take(8).Select(m => m.CommunityId));
            var playerSummaries = await steamUserService.GetPlayerSummariesAsync(steamApiKey, steamIds);

            foreach (var playerInfo in playerSummaries.Response?.Players ?? [])
            {
                var model = rankingPage.Players.FirstOrDefault(m => m.CommunityId == playerInfo.SteamId);
                if (model != null && !string.IsNullOrEmpty(playerInfo.AvatarFull))
                    model.AvatarUrl = playerInfo.AvatarFull.Replace("https://", "http://");
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }

        var stubbleBuilder = new StubbleBuilder();
        var stubble = stubbleBuilder.Build();

        _rankingTemplate ??= await EmbeddedResourceHelper.LoadEmbeddedResourceAsync("L4D2PlayStats.Core.Modules.Ranking.Resources.ranking.html");

        var html = await stubble.RenderAsync(_rankingTemplate, rankingPage);

        await using var memoryStream = new MemoryStream();
        await using var streamWriter = new StreamWriter(memoryStream);

        await streamWriter.WriteAsync(html);
        await streamWriter.FlushAsync();

        memoryStream.Position = 0;

        await azureTableStorageContext.UploadFileToBlobAsync("assets", "ranking.html", memoryStream);
    }
}