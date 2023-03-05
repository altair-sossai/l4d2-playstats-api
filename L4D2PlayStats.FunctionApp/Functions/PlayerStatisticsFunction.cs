using System;
using System.Threading.Tasks;
using L4D2PlayStats.Core.Modules.PlayerStatistics.Services;
using L4D2PlayStats.FunctionApp.Errors;
using L4D2PlayStats.FunctionApp.Extensions;
using L4D2PlayStats.FunctionApp.Shared.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace L4D2PlayStats.FunctionApp.Functions;

public class PlayerStatisticsFunction
{
    private readonly IPlayerStatisticsService _playerstatisticsService;

    public PlayerStatisticsFunction(IPlayerStatisticsService playerstatisticsService)
    {
        _playerstatisticsService = playerstatisticsService;
    }

    [FunctionName(nameof(PlayerStatisticsFunction) + "_" + nameof(PlayerStatisticsAsync))]
    public async Task<IActionResult> PlayerStatisticsAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "player-statistics/{server}")] HttpRequest httpRequest,
        string server)
    {
        try
        {
            var players = await _playerstatisticsService.PlayerStatisticsAsync(server);

            return new JsonResult(players, JsonSettings.DefaultSettings);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }
}