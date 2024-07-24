namespace L4D2PlayStats.Core.Modules.Ranking.Configs;

public class ExperienceConfig : IExperienceConfig
{
    public int DaysRange => 60;
    public int Win => 80;
    public int Loss => -45;
    public int Mvps => 8;
    public int MvpsCommon => 6;
}