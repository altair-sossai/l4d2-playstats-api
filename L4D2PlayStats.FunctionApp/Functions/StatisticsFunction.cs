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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace L4D2PlayStats.FunctionApp.Functions;

public class StatisticsFunction(
    IMapper mapper,
    IServerService serverService,
    IStatisticsService statisticsService,
    IStatisticsRepository statisticsRepository)
{
    [Function(nameof(StatisticsFunction) + "_" + nameof(GetStatisticAsync))]
    public async Task<IActionResult> GetStatisticAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "statistics/{serverId}/{statisticId}")] HttpRequest httpRequest,
        string serverId, string statisticId)
    {
        try
        {
            var statistic = await statisticsRepository.GetStatisticAsync(serverId, statisticId);
            if (statistic == null)
                return new NotFoundResult();

            var result = mapper.Map<StatisticsResult>(statistic);

            return new JsonResult(result);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [Function(nameof(StatisticsFunction) + "_" + nameof(GetStatisticsAsync))]
    public async Task<IActionResult> GetStatisticsAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "statistics/{serverId}")] HttpRequest httpRequest,
        string serverId)
    {
        try
        {
            var statistics = (await statisticsService.GetStatistics(serverId)).Take(40).ToList();

            return new JsonResult(statistics);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [Function(nameof(StatisticsFunction) + "_" + nameof(GetStatisticsBetweenAsync))]
    public async Task<IActionResult> GetStatisticsBetweenAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "statistics/{serverId}/between/{start}/and/{end}")] HttpRequest httpRequest,
        string serverId, string start, string end)
    {
        try
        {
            var statistics = await statisticsRepository
                .GetStatisticsBetweenAsync(serverId, start, end)
                .ToListAsync(CancellationToken.None);

            var result = statistics
                .OrderByDescending(o => o.RowKey)
                .Select(mapper.Map<StatisticsResult>)
                .ToList();

            return new JsonResult(result);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [Function(nameof(StatisticsFunction) + "_" + nameof(AddOrUpdate))]
    public async Task<IActionResult> AddOrUpdate([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "statistics")] HttpRequest httpRequest)
    {
        try
        {
            var server = serverService.EnsureAuthentication(httpRequest.AuthorizationToken());
            var command = await httpRequest.DeserializeBodyAsync<StatisticsCommand>();

            try
            {
                var statistic = await statisticsService.AddOrUpdateAsync(server.Id, command);
                var result = new UploadResult(statistic);

                return new JsonResult(result);
            }
            catch (ValidationException)
            {
                var date = StatisticsHelper.FileNameToDateTime(command.FileName);
                var limit = DateTime.UtcNow.AddHours(-12);
                var oldFile = limit > date;

                if (date != null && !oldFile)
                    throw;

                return new JsonResult(UploadResult.DeleteFile(command.FileName));
            }
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }
}