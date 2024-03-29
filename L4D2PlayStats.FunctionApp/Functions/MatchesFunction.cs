using System;
using System.Linq;
using System.Threading.Tasks;
using L4D2PlayStats.Core.Modules.Matches.Services;
using L4D2PlayStats.FunctionApp.Errors;
using L4D2PlayStats.FunctionApp.Extensions;
using L4D2PlayStats.FunctionApp.Shared.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace L4D2PlayStats.FunctionApp.Functions;

public class MatchesFunction
{
    private readonly IMatchService _matchService;

    public MatchesFunction(IMatchService matchService)
    {
        _matchService = matchService;
    }

    [FunctionName(nameof(MatchesFunction) + "_" + nameof(MatchesAsync))]
    public async Task<IActionResult> MatchesAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "matches/{serverId}")] HttpRequest httpRequest,
        string serverId)
    {
        try
        {
            var matches = (await _matchService.GetMatchesAsync(serverId)).Take(10).ToList();

            return new JsonResult(matches, JsonSettings.DefaultSettings);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [FunctionName(nameof(MatchesFunction) + "_" + nameof(MatchesBetweenAsync))]
    public async Task<IActionResult> MatchesBetweenAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "matches/{serverId}/between/{start}/and/{end}")] HttpRequest httpRequest,
        string serverId, string start, string end)
    {
        try
        {
            var matches = await _matchService.GetMatchesBetweenAsync(serverId, start, end);

            return new JsonResult(matches, JsonSettings.DefaultSettings);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }
}