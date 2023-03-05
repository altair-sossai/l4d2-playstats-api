using System;
using System.Linq;
using System.Threading.Tasks;
using L4D2PlayStats.Core.Modules.Matches.Extensions;
using L4D2PlayStats.Core.Modules.Matches.Services;
using L4D2PlayStats.Core.Modules.Ranking.Services;
using L4D2PlayStats.FunctionApp.Errors;
using L4D2PlayStats.FunctionApp.Extensions;
using L4D2PlayStats.FunctionApp.Shared.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace L4D2PlayStats.FunctionApp.Functions;

public class RankingFunction
{
    private readonly IMatchService _matchService;
    private readonly IRankingService _rankingService;

    public RankingFunction(IRankingService rankingService,
        IMatchService matchService)
    {
        _rankingService = rankingService;
        _matchService = matchService;
    }

    [FunctionName(nameof(RankingFunction) + "_" + nameof(RankingAsync))]
    public async Task<IActionResult> RankingAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ranking/{server}")] HttpRequest httpRequest,
        string server)
    {
        try
        {
            var players = await _rankingService.RankingAsync(server);

            return new JsonResult(players, JsonSettings.DefaultSettings);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [FunctionName(nameof(RankingFunction) + "_" + nameof(LastMatchAsync))]
    public async Task<IActionResult> LastMatchAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ranking/{server}/last-match")] HttpRequest httpRequest,
        string server)
    {
        try
        {
            var match = await _matchService.LastMatchAsync(server);
            if (match == null)
                return new NotFoundResult();

            var players = match.Ranking().ToList();
            var result = new { match, players };

            return new JsonResult(result, JsonSettings.DefaultSettings);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [FunctionName(nameof(RankingFunction) + "_" + nameof(PlaceAsync))]
    public async Task<IActionResult> PlaceAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ranking/{server}/place/{communityId:long}")] HttpRequest httpRequest,
        string server, long communityId)
    {
        try
        {
            var players = await _rankingService.RankingAsync(server);
            var top3 = players.Take(3).ToList();
            var me = players.FirstOrDefault(f => f.CommunityId == communityId);
            var result = new { top3, me };

            return new JsonResult(result, JsonSettings.DefaultSettings);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }
}