using AutoMapper;
using FluentValidation;
using L4D2PlayStats.Core.Modules.Statistics.Commands;
using L4D2PlayStats.Core.Modules.Statistics.Repositories;

namespace L4D2PlayStats.Core.Modules.Statistics.Services;

public class StatisticsService : IStatisticsService
{
    private readonly IMapper _mapper;
    private readonly IStatisticsRepository _statisticsRepository;
    private readonly IValidator<Statistics> _validator;

    public StatisticsService(IMapper mapper,
        IStatisticsRepository statisticsRepository,
        IValidator<Statistics> validator)
    {
        _mapper = mapper;
        _statisticsRepository = statisticsRepository;
        _validator = validator;
    }

    public async Task<Statistics> AddOrUpdateAsync(string server, StatisticsCommand command)
    {
        var statistics = _statisticsRepository.GetStatistic(server, command.FileName!) ?? new Statistics { Server = server };

        _mapper.Map(command, statistics);

        await _validator.ValidateAndThrowAsync(statistics);
        await _statisticsRepository.AddOrUpdateAsync(statistics);

        return statistics;
    }
}