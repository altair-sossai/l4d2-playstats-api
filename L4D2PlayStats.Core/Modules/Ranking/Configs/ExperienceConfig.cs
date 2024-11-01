using Microsoft.Extensions.Configuration;

namespace L4D2PlayStats.Core.Modules.Ranking.Configs;

public class ExperienceConfig(IConfiguration configuration) : IExperienceConfig
{
    public int Win => configuration.GetValue("ExperienceConfig:Win", 85);
    public int Loss => configuration.GetValue("ExperienceConfig:Loss", -75);
    public int RageQuit => configuration.GetValue("ExperienceConfig:RageQuit", -150);
    public int Mvps => configuration.GetValue("ExperienceConfig:Mvps", 8);
    public int MvpsCommon => configuration.GetValue("ExperienceConfig:MvpsCommon", 6);
}