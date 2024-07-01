using System;
using System.Linq;
using System.Threading.Tasks;
using L4D2PlayStats.Core.Modules.Matches.Services;
using L4D2PlayStats.FunctionApp.Errors;
using L4D2PlayStats.FunctionApp.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace L4D2PlayStats.FunctionApp.Functions;

public class MatchesFunction(IMatchService matchService)
{
    [Function(nameof(MatchesFunction) + "_" + nameof(MatchesAsync))]
    public async Task<IActionResult> MatchesAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "matches/{serverId}")] HttpRequest httpRequest,
        string serverId)
    {
        try
        {
            var matches = (await matchService.GetMatchesAsync(serverId)).Take(10).ToList();

            return new JsonResult(matches);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [Function(nameof(MatchesFunction) + "_" + nameof(MatchesBetweenAsync))]
    public async Task<IActionResult> MatchesBetweenAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "matches/{serverId}/between/{start}/and/{end}")] HttpRequest httpRequest,
        string serverId, string start, string end)
    {
        try
        {
            var matches = await matchService.GetMatchesBetweenAsync(serverId, start, end);

            return new JsonResult(matches);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }
}