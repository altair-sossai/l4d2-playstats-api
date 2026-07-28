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
    [Function($"{nameof(MatchesFunction)}_{nameof(MatchesAsync)}")]
    public async Task<IActionResult> MatchesAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "matches/{serverId}")] HttpRequest httpRequest,
        string serverId, int count = 20)
    {
        try
        {
            var matches = await matchService.GetMatchesAsync(serverId);
            var result = matches.Take(count).ToList();

            return new JsonResult(result);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [Function($"{nameof(MatchesFunction)}_{nameof(MatchesForYearAsync)}")]
    public async Task<IActionResult> MatchesForYearAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "matches/{serverId}/year/{year}")] HttpRequest httpRequest,
        string serverId, int year)
    {
        try
        {
            var start = new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var end = new DateTime(year, 12, 31, 23, 59, 59, DateTimeKind.Utc);
            var matches = await matchService.GetMatchesAsync(serverId, start, end);
            var result = matches.ToList();

            return new JsonResult(result);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [Function($"{nameof(MatchesFunction)}_{nameof(MatchesBetweenAsync)}")]
    public async Task<IActionResult> MatchesBetweenAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "matches/{serverId}/between/{start}/and/{end}")] HttpRequest httpRequest,
        string serverId, string start, string end)
    {
        try
        {
            var matches = await matchService.GetMatchesAsync(serverId, start, end);

            return new JsonResult(matches);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }
}