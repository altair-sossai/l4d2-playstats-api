using System;
using System.Linq;
using System.Threading.Tasks;
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
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.FunctionApp.Functions;

public class ServerFunction
{
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;
    private readonly IServerService _serverService;

    public ServerFunction(IMemoryCache memoryCache,
        IMapper mapper,
        IServerService serverService)
    {
        _memoryCache = memoryCache;
        _mapper = mapper;
        _serverService = serverService;
    }

    [FunctionName(nameof(ServerFunction) + "_" + nameof(Servers))]
    public async Task<IActionResult> Servers([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "servers")] HttpRequest httpRequest)
    {
        try
        {
            var result = await _memoryCache.GetOrCreateAsync("servers".ToLower(), factory =>
            {
                factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

                var servers = _serverService.GetServers();
                var result = servers.Select(_mapper.Map<ServerResult>).ToList();

                return Task.FromResult(result);
            });

            return new JsonResult(result, JsonSettings.DefaultSettings);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [FunctionName(nameof(ServerFunction) + "_" + nameof(Server))]
    public async Task<IActionResult> Server([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "servers/{serverId}")] HttpRequest httpRequest,
        string serverId)
    {
        try
        {
            var server = await _memoryCache.GetOrCreateAsync($"servers_{serverId.ToLower()}", factory =>
            {
                factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

                var server = _serverService.GetServer(serverId);

                return Task.FromResult(server == null ? null : _mapper.Map<ServerResult>(server));
            });

            if (server == null)
                return new NotFoundResult();

            var result = _mapper.Map<ServerResult>(server);

            return new JsonResult(result, JsonSettings.DefaultSettings);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }
}