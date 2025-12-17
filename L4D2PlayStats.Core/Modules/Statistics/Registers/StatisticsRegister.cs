using L4D2PlayStats.Core.Modules.Statistics.Commands;
using L4D2PlayStats.Core.Modules.Statistics.Results;
using Mapster;

namespace L4D2PlayStats.Core.Modules.Statistics.Registers;

public class StatisticsRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Statistics, StatisticsResult>()
            .Map(dest => dest.StatisticId, src => src.RowKey);

        config.NewConfig<Statistics, StatisticsSimplifiedResult>()
            .Map(dest => dest.StatisticId, src => src.RowKey)
            .Map(dest => dest.GameRound, src => src.Statistic!.GameRound)
            .Map(dest => dest.Scoring, src => src.Statistic!.Scoring)
            .Map(dest => dest.TeamA, src => src.Statistic!.TeamA)
            .Map(dest => dest.TeamB, src => src.Statistic!.TeamB)
            .Map(dest => dest.MapStart, src => src.Statistic!.MapStart)
            .Map(dest => dest.MapEnd, src => src.Statistic!.MapEnd)
            .Map(dest => dest.MapElapsed, src => src.Statistic!.MapElapsed);

        config.NewConfig<StatisticsCommand, Statistics>();
    }
}