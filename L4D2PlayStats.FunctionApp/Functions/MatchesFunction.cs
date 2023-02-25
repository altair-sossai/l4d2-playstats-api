using System;
using System.Threading.Tasks;
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

public class MatchesFunction
{
    private readonly IMatchService _matchService;
    private readonly IMemoryCache _memoryCache;

    public MatchesFunction(IMemoryCache memoryCache, IMatchService matchService)
    {
        _memoryCache = memoryCache;
        _matchService = matchService;
    }

    [FunctionName(nameof(MatchesFunction) + "_" + nameof(GetMatchesAsync))]
    public async Task<IActionResult> GetMatchesAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "matches/{server}")] HttpRequest httpRequest,
        string server)
    {
        try
        {
            var results = await _memoryCache.GetOrCreateAsync($"matches_{server}".ToLower(), async factory =>
            {
                factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

                var matches = await _matchService.GetMatchesAsync(server);

                return matches;
            });

            return new JsonResult(results, JsonSettings.DefaultSettings);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [FunctionName(nameof(MatchesFunction) + "_" + nameof(GetMatchesBetweenAsync))]
    public async Task<IActionResult> GetMatchesBetweenAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "matches/{server}/between/{start}/and/{end}")] HttpRequest httpRequest,
        string server, string start, string end)
    {
        try
        {
            var matches = await _matchService.GetMatchesBetweenAsync(server, start, end);

            return new JsonResult(matches, JsonSettings.DefaultSettings);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }
}