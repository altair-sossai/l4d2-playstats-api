using L4D2PlayStats.Core.Modules.Ranking.Configs;

namespace L4D2PlayStats.Core.Modules.Ranking.Structures;

public class ExperienceCalculation(IExperienceConfig config)
{
    public bool Win { get; set; }
    public bool Loss { get; set; }
    public bool RageQuit { get; set; }
    public int Mvps { get; set; }
    public int MvpsCommon { get; set; }

    public decimal Experience
    {
        get
        {
            if (RageQuit)
                return config.RageQuit;

            var experience = 0m;

            if (Win)
                experience += config.Win;

            if (Loss)
                experience += config.Loss;

            experience += Mvps * config.Mvps;
            experience += MvpsCommon * config.MvpsCommon;

            return experience;
        }
    }
}