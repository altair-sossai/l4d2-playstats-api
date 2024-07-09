using L4D2PlayStats.Core.Contexts.AzureTableStorage;
using L4D2PlayStats.Core.Contexts.Steam;
using L4D2PlayStats.Core.Modules.Matches.Services;
using L4D2PlayStats.Core.Modules.Ranking.Extensions;
using L4D2PlayStats.Core.Modules.Ranking.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace L4D2PlayStats.Core.Modules.Ranking.Services;

public class RankingService(
    IMemoryCache memoryCache,
    IMatchService matchService,
    IAzureTableStorageContext azureTableStorageContext,
    ISteamContext steamContext,
    IConfiguration configuration) : IRankingService
{
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

        await rankingPage.UpdatePlayersAvatarUrlAsync(steamUserService, steamApiKey);

        await using var stream = await rankingPage.RenderAsync();

        await azureTableStorageContext.UploadHtmlFileToBlobAsync("assets", "ranking.html", stream);
    }
}