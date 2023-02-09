using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using L4D2PlayStats.Core.Modules.Auth.Users.Services;
using L4D2PlayStats.Core.Modules.Statistics.Commands;
using L4D2PlayStats.Core.Modules.Statistics.Repositories;
using L4D2PlayStats.Core.Modules.Statistics.Results;
using L4D2PlayStats.Core.Modules.Statistics.Services;
using L4D2PlayStats.FunctionApp.Errors;
using L4D2PlayStats.FunctionApp.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace L4D2PlayStats.FunctionApp.Functions;

public class StatisticsFunction
{
    private readonly IMapper _mapper;
    private readonly IStatisticsRepository _statisticsRepository;
    private readonly IStatisticsService _statisticsService;
    private readonly IUserService _userService;

    public StatisticsFunction(IMapper mapper,
        IUserService userService,
        IStatisticsService statisticsService,
        IStatisticsRepository statisticsRepository)
    {
        _mapper = mapper;
        _userService = userService;
        _statisticsService = statisticsService;
        _statisticsRepository = statisticsRepository;
    }

    [FunctionName(nameof(StatisticsFunction) + "_" + nameof(GetStatistics))]
    public IActionResult GetStatistics([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "statistics/{server}")] HttpRequest httpRequest,
        string server)
    {
        try
        {
            var results = _statisticsRepository
                .GetStatistics(server)
                .Select(_mapper.Map<StatisticsResult>)
                .ToList();

            return new OkObjectResult(results);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [FunctionName(nameof(StatisticsFunction) + "_" + nameof(GetStatistic))]
    public IActionResult GetStatistic([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "statistics/{server}/{fileName}")] HttpRequest httpRequest,
        string server, string fileName)
    {
        try
        {
            var statistic = _statisticsRepository.GetStatistic(server, fileName);
            if (statistic == null)
                return new NotFoundResult();

            var result = _mapper.Map<StatisticsResult>(statistic);

            return new OkObjectResult(result);
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
            var user = _userService.EnsureAuthentication(httpRequest.AuthorizationToken());
            var command = await httpRequest.DeserializeBodyAsync<StatisticsCommand>();
            var statistic = await _statisticsService.AddOrUpdateAsync(user.Id, command);
            var result = new UploadResult(statistic);

            return new OkObjectResult(result);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }
}