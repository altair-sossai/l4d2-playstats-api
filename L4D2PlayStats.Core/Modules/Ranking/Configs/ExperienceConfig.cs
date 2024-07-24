namespace L4D2PlayStats.Core.Modules.Ranking.Configs;

public class ExperienceConfig : IExperienceConfig
{
    public int DaysRange => 60;
    public int Win => 65;
    public int Loss => -80;
    public int Mvps => 7;
    public int MvpsCommon => 5;
}