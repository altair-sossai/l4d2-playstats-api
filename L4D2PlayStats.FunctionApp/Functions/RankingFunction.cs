using System;
using System.Linq;
using System.Threading.Tasks;
using L4D2PlayStats.Core.Modules.Matches.Services;
using L4D2PlayStats.Core.Modules.Ranking.Configs;
using L4D2PlayStats.Core.Modules.Ranking.Extensions;
using L4D2PlayStats.Core.Modules.Ranking.Services;
using L4D2PlayStats.FunctionApp.Errors;
using L4D2PlayStats.FunctionApp.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace L4D2PlayStats.FunctionApp.Functions;

public class RankingFunction(IRankingService rankingService, IMatchService matchService, IExperienceConfig config)
{
    [Function($"{nameof(RankingFunction)}_{nameof(RankingAsync)}")]
    public async Task<IActionResult> RankingAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ranking/{serverId}")] HttpRequest httpRequest,
        string serverId)
    {
        try
        {
            var players = await rankingService.RankingAsync(serverId);

            return new JsonResult(players);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [Function($"{nameof(RankingFunction)}_{nameof(LastMatchAsync)}")]
    public async Task<IActionResult> LastMatchAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ranking/{serverId}/last-match")] HttpRequest httpRequest,
        string serverId)
    {
        try
        {
            var match = await matchService.LastMatchAsync(serverId);
            if (match == null)
                return new NotFoundResult();

            var players = match.Ranking(config).ToList();
            var result = new { match, players };

            return new JsonResult(result);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [Function($"{nameof(RankingFunction)}_{nameof(PlaceAsync)}")]
    public async Task<IActionResult> PlaceAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ranking/{serverId}/place/{communityId:long}")] HttpRequest httpRequest,
        string serverId, long communityId)
    {
        try
        {
            var players = await rankingService.RankingAsync(serverId);
            var top3 = players.Take(3).ToList();
            var me = players.FirstOrDefault(f => f.CommunityId == communityId);
            var result = new { top3, me };

            return new JsonResult(result);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [Function($"{nameof(RankingFunction)}_{nameof(ConfigAsync)}")]
    public IActionResult ConfigAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ranking-config")] HttpRequest httpRequest)
    {
        try
        {
            return new JsonResult(config);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }
}