using System;
using System.Threading.Tasks;
using L4D2PlayStats.Core.Modules.Server.Services;
using L4D2PlayStats.Core.Modules.Stars.Services;
using L4D2PlayStats.FunctionApp.Errors;
using L4D2PlayStats.FunctionApp.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace L4D2PlayStats.FunctionApp.Functions;

public class StarsFunction(IServerService serverService, IStarsService starsService)
{
    [Function($"{nameof(StarsFunction)}_{nameof(StarsAsync)}")]
    public async Task<IActionResult> StarsAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "stars/{serverId}")] HttpRequest httpRequest,
        string serverId)
    {
        try
        {
            var players = await starsService.StarsAsync(serverId);

            return new JsonResult(players);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [Function($"{nameof(StarsFunction)}_{nameof(PlayerStarsAsync)}")]
    public async Task<IActionResult> PlayerStarsAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "stars/{serverId}/{communityId:long}")] HttpRequest httpRequest,
        string serverId, long communityId)
    {
        try
        {
            var player = await starsService.StarsAsync(serverId, communityId);

            return new JsonResult(player);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [Function($"{nameof(StarsFunction)}_{nameof(SaveClosedStarsAsync)}")]
    public async Task SaveClosedStarsAsync([TimerTrigger("0 30 0 * * *")] TimerInfo timerInfo)
    {
        foreach (var server in serverService.GetServers())
            await starsService.SaveClosedStarsAsync(server.Id);
    }
}