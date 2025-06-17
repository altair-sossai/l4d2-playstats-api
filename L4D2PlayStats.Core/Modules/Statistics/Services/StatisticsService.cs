using AutoMapper;
using FluentValidation;
using L4D2PlayStats.Core.Modules.Statistics.Commands;
using L4D2PlayStats.Core.Modules.Statistics.Repositories;
using L4D2PlayStats.Core.Modules.Statistics.Results;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Core.Modules.Statistics.Services;

public class StatisticsService(
    IMapper mapper,
    IMemoryCache memoryCache,
    IStatisticsRepository statisticsRepository,
    IValidator<Statistics> statisticsValidator,
    IValidator<UpdateScoreCommand> updateScoreValidator)
    : IStatisticsService
{
    public async Task<List<StatisticsSimplifiedResult>> GetStatistics(string serverId)
    {
        var statistics = await memoryCache.GetOrCreateAsync($"statistics_{serverId}".ToLower(), async factory =>
        {
            factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

            var statistics = await statisticsRepository
                .GetStatisticsAsync(serverId)
                .ToListAsync(CancellationToken.None);

            return statistics.Select(mapper.Map<StatisticsSimplifiedResult>).ToList();
        });

        return statistics!;
    }

    public async Task<Statistics> AddOrUpdateAsync(string serverId, StatisticsCommand command)
    {
        if (string.IsNullOrEmpty(command.FileName))
            throw new Exception("Invalid filename");

        if (string.IsNullOrEmpty(command.Content) || !L4D2PlayStats.Statistics.TryParse(command.Content, out _))
            throw new Exception("Invalid content");

        var statistics = await statisticsRepository.GetStatisticAsync(serverId, command.FileName!) ?? new Statistics { Server = serverId };

        mapper.Map(command, statistics);

        await statisticsValidator.ValidateAndThrowAsync(statistics);
        await statisticsRepository.AddOrUpdateAsync(statistics);

        ClearMemoryCache(serverId);

        return statistics;
    }

    public async Task<Statistics> UpdateScoreAsync(string serverId, string statisticId, UpdateScoreCommand command)
    {
        await updateScoreValidator.ValidateAndThrowAsync(command);

        var statistic = await statisticsRepository.GetStatisticAsync(serverId, statisticId);
        if (statistic == null)
            throw new Exception("Statistic not found");

        statistic.UpdateScore(command.TeamAScore, command.TeamBScore);

        await statisticsValidator.ValidateAndThrowAsync(statistic);
        await statisticsRepository.AddOrUpdateAsync(statistic);

        ClearMemoryCache(serverId);

        return statistic;
    }

    private void ClearMemoryCache(string serverId)
    {
        memoryCache.Remove($"statistics_{serverId}".ToLower());
        memoryCache.Remove($"matches_{serverId}".ToLower());
        memoryCache.Remove($"ranking_{serverId}".ToLower());
        memoryCache.Remove($"ranking_last_match_{serverId}".ToLower());
        memoryCache.Remove($"player_statistics_{serverId}".ToLower());
    }
}