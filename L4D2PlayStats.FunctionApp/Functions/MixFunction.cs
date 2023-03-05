using System;
using System.Threading.Tasks;
using L4D2PlayStats.Core.Modules.Mix.Commands;
using L4D2PlayStats.Core.Modules.Mix.Services;
using L4D2PlayStats.Core.Modules.Server.Services;
using L4D2PlayStats.FunctionApp.Errors;
using L4D2PlayStats.FunctionApp.Extensions;
using L4D2PlayStats.FunctionApp.Shared.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace L4D2PlayStats.FunctionApp.Functions;

public class MixFunction
{
    private readonly IMixService _mixService;
    private readonly IServerService _serverService;

    public MixFunction(IServerService serverService,
        IMixService mixService)
    {
        _serverService = serverService;
        _mixService = mixService;
    }

    [FunctionName(nameof(MixFunction) + "_" + nameof(MixAsync))]
    public async Task<IActionResult> MixAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "mix")] HttpRequest httpRequest)
    {
        try
        {
            var server = _serverService.EnsureAuthentication(httpRequest.AuthorizationToken());
            var command = await httpRequest.DeserializeBodyAsync<MixCommand>();
            var result = await _mixService.MixAsync(server.Id, command);

            return new JsonResult(result, JsonSettings.DefaultSettings);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }
}