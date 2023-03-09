using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using L4D2PlayStats.Core.Modules.Server.Services;
using L4D2PlayStats.Core.Modules.Statistics.Commands;
using L4D2PlayStats.Core.Modules.Statistics.Helpers;
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

namespace L4D2PlayStats.FunctionApp.Functions;

public class StatisticsFunction
{
    private readonly IMapper _mapper;
    private readonly IServerService _serverService;
    private readonly IStatisticsRepository _statisticsRepository;
    private readonly IStatisticsService _statisticsService;

    public StatisticsFunction(IMapper mapper,
        IServerService serverService,
        IStatisticsService statisticsService,
        IStatisticsRepository statisticsRepository)
    {
        _mapper = mapper;
        _serverService = serverService;
        _statisticsService = statisticsService;
        _statisticsRepository = statisticsRepository;
    }

    [FunctionName(nameof(StatisticsFunction) + "_" + nameof(GetStatisticAsync))]
    public async Task<IActionResult> GetStatisticAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "statistics/{serverId}/{statisticId}")] HttpRequest httpRequest,
        string serverId, string statisticId)
    {
        try
        {
            var statistic = await _statisticsRepository.GetStatisticAsync(serverId, statisticId);
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
    public async Task<IActionResult> GetStatisticsAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "statistics/{serverId}")] HttpRequest httpRequest,
        string serverId)
    {
        try
        {
            var statistics = (await _statisticsService.GetStatistics(serverId)).Take(200).ToList();

            return new JsonResult(statistics, JsonSettings.DefaultSettings);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [FunctionName(nameof(StatisticsFunction) + "_" + nameof(GetStatisticsBetweenAsync))]
    public async Task<IActionResult> GetStatisticsBetweenAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "statistics/{serverId}/between/{start}/and/{end}")] HttpRequest httpRequest,
        string serverId, string start, string end)
    {
        try
        {
            var statistics = await _statisticsRepository
                .GetStatisticsBetweenAsync(serverId, start, end)
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

            try
            {
                var statistic = await _statisticsService.AddOrUpdateAsync(server.Id, command);
                var result = new UploadResult(statistic);

                return new JsonResult(result, JsonSettings.DefaultSettings);
            }
            catch (ValidationException)
            {
                var date = StatisticsHelper.FileNameToDateTime(command.FileName);
                var limit = DateTime.UtcNow.AddHours(-12);
                var oldFile = limit > date;

                if (date != null && !oldFile)
                    throw;

                return new JsonResult(UploadResult.DeleteFile(command.FileName), JsonSettings.DefaultSettings);
            }
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }
}