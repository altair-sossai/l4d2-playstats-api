using Microsoft.Extensions.Configuration;

namespace L4D2PlayStats.Core.Modules.Ranking.Configs;

public class ExperienceConfig(IConfiguration configuration) : IExperienceConfig
{
    public int DaysRange => configuration.GetValue("ExperienceConfig:DaysRange", 60);
    public int Win => configuration.GetValue("ExperienceConfig:Win", 80);
    public int Loss => configuration.GetValue("ExperienceConfig:Loss", -65);
    public int Mvps => configuration.GetValue("ExperienceConfig:Mvps", 7);
    public int MvpsCommon => configuration.GetValue("ExperienceConfig:MvpsCommon", 5);
}