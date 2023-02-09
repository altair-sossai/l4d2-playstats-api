using AutoMapper;
using L4D2PlayStats.Core.Modules.Statistics.Commands;
using L4D2PlayStats.Core.Modules.Statistics.Results;

namespace L4D2PlayStats.Core.Modules.Statistics.Profiles;

public class StatisticsProfile : Profile
{
    public StatisticsProfile()
    {
        CreateMap<Statistics, StatisticsResult>();
        CreateMap<StatisticsCommand, Statistics>();
    }
}