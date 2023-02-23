using System;
using System.Linq;
using System.Threading.Tasks;
using L4D2PlayStats.Core.Modules.Matches.Extensions;
using L4D2PlayStats.Core.Modules.Matches.Services;
using L4D2PlayStats.FunctionApp.Errors;
using L4D2PlayStats.FunctionApp.Extensions;
using L4D2PlayStats.FunctionApp.Shared.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.FunctionApp.Functions;

public class RankingFunction
{
    private readonly IMatchService _matchService;
    private readonly IMemoryCache _memoryCache;

    public RankingFunction(IMemoryCache memoryCache,
        IMatchService matchService)
    {
        _memoryCache = memoryCache;
        _matchService = matchService;
    }

    [FunctionName(nameof(RankingFunction) + "_" + nameof(RankingAsync))]
    public async Task<IActionResult> RankingAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ranking/{server}")] HttpRequest httpRequest,
        string server)
    {
        try
        {
            var players = await _memoryCache.GetOrCreateAsync($"ranking_{server}".ToLower(), async factory =>
            {
                factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

                var after = DateTime.UtcNow.AddDays(-30);
                var matches = await _matchService.GetMatchesAsync(server, after);
                var players = matches.Ranking().ToList();

                return players;
            });

            return new JsonResult(players, JsonSettings.DefaultSettings);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }
}