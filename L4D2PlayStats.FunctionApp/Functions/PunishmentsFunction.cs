using System;
using System.Threading.Tasks;
using L4D2PlayStats.Core.Modules.Punishments.Commands;
using L4D2PlayStats.Core.Modules.Punishments.Services;
using L4D2PlayStats.Core.Modules.Server.Services;
using L4D2PlayStats.FunctionApp.Errors;
using L4D2PlayStats.FunctionApp.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace L4D2PlayStats.FunctionApp.Functions;

public class PunishmentsFunction(
    IServerService serverService,
    IPunishmentsService punishmentsService)
{
    [Function($"{nameof(PunishmentsFunction)}_{nameof(AddOrUpdateAsync)}")]
    public async Task<IActionResult> AddOrUpdateAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "punishments")] HttpRequest httpRequest)
    {
        try
        {
            var server = serverService.EnsureAuthentication(httpRequest.AuthorizationToken());
            var command = await httpRequest.DeserializeBodyAsync<PunishmentCommand>();
            var punishment = await punishmentsService.AddOrUpdateAsync(server.Id, command);

            return new JsonResult(punishment);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [Function($"{nameof(PunishmentsFunction)}_{nameof(DeleteAsync)}")]
    public async Task<IActionResult> DeleteAsync([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "punishments/{communityId}")] HttpRequest httpRequest, string communityId)
    {
        try
        {
            var server = serverService.EnsureAuthentication(httpRequest.AuthorizationToken());
            await punishmentsService.DeleteAsync(server.Id, communityId);

            return new OkResult();
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }
}