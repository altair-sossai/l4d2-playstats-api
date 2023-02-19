using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using L4D2PlayStats.Core.Modules.Server.Services;
using L4D2PlayStats.Core.Modules.Statistics.Commands;
using L4D2PlayStats.Core.Modules.Statistics.Repositories;
using L4D2PlayStats.Core.Modules.Statistics.Results;
using L4D2PlayStats.Core.Modules.Statistics.Services;
using L4D2PlayStats.FunctionApp.Errors;
using L4D2PlayStats.FunctionApp.Extensions;
using L4D2PlayStats.FunctionApp.Shared.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.FunctionApp.Functions;

public class StatisticsFunction
{
    private const int StatisticsCount = 250;

    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;
    private readonly IServerService _serverService;
    private readonly IStatisticsRepository _statisticsRepository;
    private readonly IStatisticsService _statisticsService;

    public StatisticsFunction(IMemoryCache memoryCache,
        IMapper mapper,
        IServerService serverService,
        IStatisticsService statisticsService,
        IStatisticsRepository statisticsRepository)
    {
        _memoryCache = memoryCache;
        _mapper = mapper;
        _serverService = serverService;
        _statisticsService = statisticsService;
        _statisticsRepository = statisticsRepository;
    }

    [FunctionName(nameof(StatisticsFunction) + "_" + nameof(GetStatisticAsync))]
    public async Task<IActionResult> GetStatisticAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "statistics/{server}/{statisticId}")] HttpRequest httpRequest,
        string server, string statisticId)
    {
        try
        {
            var statistic = await _statisticsRepository.GetStatisticAsync(server, statisticId);
            if (statistic == null)
                return new NotFoundResult();

            var result = _mapper.Map<StatisticsResult>(statistic);

            return new JsonResult(result, JsonSettings.DefaultSettings);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [FunctionName(nameof(StatisticsFunction) + "_" + nameof(GetStatisticsAsync))]
    public async Task<IActionResult> GetStatisticsAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "statistics/{server}")] HttpRequest httpRequest,
        string server)
    {
        try
        {
            var results = await _memoryCache.GetOrCreateAsync($"statistics_{server}".ToLower(), async factory =>
            {
                factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

                var statistics = await _statisticsRepository
                    .GetStatisticsAsync(server)
                    .Take(StatisticsCount)
                    .ToListAsync(CancellationToken.None);

                return statistics.Select(_mapper.Map<StatisticsSimplifiedResult>).ToList();
            });

            return new JsonResult(results, JsonSettings.DefaultSettings);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [FunctionName(nameof(StatisticsFunction) + "_" + nameof(GetStatisticsBetweenAsync))]
    public async Task<IActionResult> GetStatisticsBetweenAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "statistics/{server}/between/{start}/and/{end}")] HttpRequest httpRequest,
        string server, string start, string end)
    {
        try
        {
            var statistics = await _statisticsRepository
                .GetStatisticsBetweenAsync(server, start, end)
                .ToListAsync(CancellationToken.None);

            var result = statistics
                .OrderByDescending(o => o.RowKey)
                .Select(_mapper.Map<StatisticsResult>)
                .ToList();

            return new JsonResult(result, JsonSettings.DefaultSettings);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [FunctionName(nameof(StatisticsFunction) + "_" + nameof(AddOrUpdate))]
    public async Task<IActionResult> AddOrUpdate([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "statistics")] HttpRequest httpRequest)
    {
        try
        {
            var server = _serverService.EnsureAuthentication(httpRequest.AuthorizationToken());
            var command = await httpRequest.DeserializeBodyAsync<StatisticsCommand>();
            var statistic = await _statisticsService.AddOrUpdateAsync(server.Id, command);
            var result = new UploadResult(statistic);

            _memoryCache.Remove($"statistics_{server.Id}".ToLower());

            return new JsonResult(result, JsonSettings.DefaultSettings);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }
}