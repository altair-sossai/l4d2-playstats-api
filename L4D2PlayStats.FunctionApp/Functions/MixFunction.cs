using System;
using System.Threading.Tasks;
using L4D2PlayStats.Core.Modules.Mix.Commands;
using L4D2PlayStats.Core.Modules.Mix.Services;
using L4D2PlayStats.Core.Modules.Server.Services;
using L4D2PlayStats.FunctionApp.Errors;
using L4D2PlayStats.FunctionApp.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace L4D2PlayStats.FunctionApp.Functions;

public class MixFunction(IServerService serverService, IMixService mixService)
{
    [Function($"{nameof(MixFunction)}_{nameof(MixAsync)}")]
    public async Task<IActionResult> MixAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "mix")] HttpRequest httpRequest)
    {
        try
        {
            var server = serverService.EnsureAuthentication(httpRequest.AuthorizationToken());
            var command = await httpRequest.DeserializeBodyAsync<MixCommand>();
            var result = await mixService.MixAsync(server.Id, command);

            return new JsonResult(result);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }
}