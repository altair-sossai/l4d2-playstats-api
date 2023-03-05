using AutoMapper;
using FluentValidation;
using L4D2PlayStats.Core.Modules.Statistics.Commands;
using L4D2PlayStats.Core.Modules.Statistics.Repositories;
using L4D2PlayStats.Core.Modules.Statistics.Results;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Core.Modules.Statistics.Services;

public class StatisticsService : IStatisticsService
{
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;
    private readonly IStatisticsRepository _statisticsRepository;
    private readonly IValidator<Statistics> _validator;

    public StatisticsService(IMapper mapper,
        IMemoryCache memoryCache,
        IStatisticsRepository statisticsRepository,
        IValidator<Statistics> validator)
    {
        _mapper = mapper;
        _memoryCache = memoryCache;
        _statisticsRepository = statisticsRepository;
        _validator = validator;
    }

    public async Task<Statistics> AddOrUpdateAsync(string server, StatisticsCommand command)
    {
        if (string.IsNullOrEmpty(command.FileName))
            throw new Exception("Invalid filename");

        if (string.IsNullOrEmpty(command.Content) || !L4D2PlayStats.Statistics.TryParse(command.Content, out _))
            throw new Exception("Invalid content");

        var statistics = await _statisticsRepository.GetStatisticAsync(server, command.FileName!) ?? new Statistics { Server = server };

        _mapper.Map(command, statistics);

        await _validator.ValidateAndThrowAsync(statistics);
        await _statisticsRepository.AddOrUpdateAsync(statistics);

        ClearMemoryCache(server);

        return statistics;
    }

    public async Task<List<StatisticsSimplifiedResult>> GetStatistics(string server)
    {
        var statistics = await _memoryCache.GetOrCreateAsync($"statistics_{server}".ToLower(), async factory =>
        {
            factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

            var statistics = await _statisticsRepository
                .GetStatisticsAsync(server)
                .ToListAsync(CancellationToken.None);

            return statistics.Select(_mapper.Map<StatisticsSimplifiedResult>).ToList();
        });

        return statistics;
    }

    private void ClearMemoryCache(string server)
    {
        _memoryCache.Remove($"statistics_{server}".ToLower());
        _memoryCache.Remove($"matches_{server}".ToLower());
        _memoryCache.Remove($"ranking_{server}".ToLower());
        _memoryCache.Remove($"ranking_last_match_{server}".ToLower());
    }
}