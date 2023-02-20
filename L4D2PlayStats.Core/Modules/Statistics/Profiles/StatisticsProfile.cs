using AutoMapper;
using L4D2PlayStats.Core.Modules.Statistics.Commands;
using L4D2PlayStats.Core.Modules.Statistics.Results;

namespace L4D2PlayStats.Core.Modules.Statistics.Profiles;

public class StatisticsProfile : Profile
{
    public StatisticsProfile()
    {
        CreateMap<Statistics, StatisticsResult>()
            .ForMember(dest => dest.StatisticId, opt => opt.MapFrom(src => src.RowKey));

        CreateMap<Statistics, StatisticsSimplifiedResult>()
            .ForMember(dest => dest.StatisticId, opt => opt.MapFrom(src => src.RowKey))
            .ForMember(dest => dest.GameRound, opt => opt.MapFrom(src => src.Statistic!.GameRound))
            .ForMember(dest => dest.Scoring, opt => opt.MapFrom(src => src.Statistic!.Scoring))
            .ForMember(dest => dest.PlayerNames, opt => opt.MapFrom(src => src.Statistic!.PlayerNames))
            .ForMember(dest => dest.MapStart, opt => opt.MapFrom(src => src.Statistic!.MapStart))
            .ForMember(dest => dest.MapEnd, opt => opt.MapFrom(src => src.Statistic!.MapEnd))
            .ForMember(dest => dest.MapElapsed, opt => opt.MapFrom(src => src.Statistic!.MapElapsed));

        CreateMap<StatisticsCommand, Statistics>();
    }
}