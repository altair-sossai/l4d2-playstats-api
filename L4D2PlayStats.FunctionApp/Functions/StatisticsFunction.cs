using System;
using System.Linq;
using System.Threading;
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
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.FunctionApp.Functions;

public class StatisticsFunction
{
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;
    private readonly IStatisticsRepository _statisticsRepository;
    private readonly IStatisticsService _statisticsService;
    private readonly IUserService _userService;

    public StatisticsFunction(IMemoryCache memoryCache,
        IMapper mapper,
        IUserService userService,
        IStatisticsService statisticsService,
        IStatisticsRepository statisticsRepository)
    {
        _memoryCache = memoryCache;
        _mapper = mapper;
        _userService = userService;
        _statisticsService = statisticsService;
        _statisticsRepository = statisticsRepository;
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
                    .Take(100)
                    .ToListAsync(CancellationToken.None);

                return statistics.Select(_mapper.Map<StatisticsSimplifiedResult>).ToList();
            });

            return new OkObjectResult(results);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }

    [FunctionName(nameof(StatisticsFunction) + "_" + nameof(GetStatisticAsync))]
    public async Task<IActionResult> GetStatisticAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "statistics/{server}/{fileName}")] HttpRequest httpRequest,
        string server, string fileName)
    {
        try
        {
            var statistic = await _statisticsRepository.GetStatisticAsync(server, fileName);
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

            _memoryCache.Remove($"statistics_{user.Id}".ToLower());

            return new OkObjectResult(result);
        }
        catch (Exception exception)
        {
            return ErrorResult.Build(exception).ResponseMessageResult();
        }
    }
}