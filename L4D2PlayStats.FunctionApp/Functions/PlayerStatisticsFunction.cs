using System;
using System.Threading.Tasks;
using L4D2PlayStats.Core.Modules.PlayerStatistics.Services;
using L4D2PlayStats.FunctionApp.Errors;
using L4D2PlayStats.FunctionApp.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace L4D2PlayStats.FunctionApp.Functions;

public class PlayerStatisticsFunction(IPlayerStatisticsService playerstatisticsService)
{
    [Function($"{nameof(PlayerStatisticsFunction)}_{nameof(PlayerStatisticsAsync)}")]
    public async Task<IActionResult> PlayerStatisticsAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "player-statistics/{serverId}")] HttpRequest httpRequest,
        string serverId)
    {
        try
        {
            var players = await playerstatisticsService.PlayerStatisticsAsync(serverId);

            return new JsonResult(players);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }
}