using AutoMapper;
using L4D2PlayStats.Core.Modules.Statistics.Commands;
using L4D2PlayStats.Core.Modules.Statistics.Results;

namespace L4D2PlayStats.Core.Modules.Statistics.Profiles;

public class StatisticsProfile : Profile
{
    public StatisticsProfile()
    {
        CreateMap<Statistics, StatisticsResult>();
        CreateMap<Statistics, StatisticsSimplifiedResult>()
            .ForMember(dest => dest.GameRound, opt => opt.MapFrom(src => src.Statistic!.GameRound))
            .ForMember(dest => dest.Scoring, opt => opt.MapFrom(src => src.Statistic!.Scoring))
            .ForMember(dest => dest.PlayerNames, opt => opt.MapFrom(src => src.Statistic!.PlayerNames));

        CreateMap<StatisticsCommand, Statistics>();
    }
}