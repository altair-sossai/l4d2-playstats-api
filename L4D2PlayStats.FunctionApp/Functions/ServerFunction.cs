using System;
using System.Linq;
using AutoMapper;
using L4D2PlayStats.Core.Modules.Server.Results;
using L4D2PlayStats.Core.Modules.Server.Services;
using L4D2PlayStats.FunctionApp.Errors;
using L4D2PlayStats.FunctionApp.Extensions;
using L4D2PlayStats.FunctionApp.Shared.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace L4D2PlayStats.FunctionApp.Functions;

public class ServerFunction(
    IMapper mapper,
    IServerService serverService)
{
    [FunctionName(nameof(ServerFunction) + "_" + nameof(Servers))]
    public IActionResult Servers([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "servers")] HttpRequest httpRequest)
    {
        try
        {
            var servers = serverService.GetServers();
            var result = servers.Select(mapper.Map<ServerResult>).ToList();

            return new JsonResult(result, JsonSettings.DefaultSettings);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [FunctionName(nameof(ServerFunction) + "_" + nameof(Server))]
    public IActionResult Server([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "servers/{serverId}")] HttpRequest httpRequest,
        string serverId)
    {
        try
        {
            var server = serverService.GetServer(serverId);
            if (server == null)
                return new NotFoundResult();

            var result = mapper.Map<ServerResult>(server);

            return new JsonResult(result, JsonSettings.DefaultSettings);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }
}