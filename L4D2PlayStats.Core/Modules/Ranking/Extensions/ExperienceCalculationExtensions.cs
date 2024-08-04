using L4D2PlayStats.Core.Modules.Ranking.Configs;
using L4D2PlayStats.Core.Modules.Ranking.Structures;

namespace L4D2PlayStats.Core.Modules.Ranking.Extensions;

public static class ExperienceCalculationExtensions
{
    public static void Win(this Dictionary<string, ExperienceCalculation> playersExperience, string? communityId, IExperienceConfig config)
    {
        if (string.IsNullOrEmpty(communityId))
            return;

        if (!playersExperience.ContainsKey(communityId))
            playersExperience.Add(communityId, new ExperienceCalculation(config));

        playersExperience[communityId].Win = true;
    }

    public static void Loss(this Dictionary<string, ExperienceCalculation> playersExperience, string? communityId, IExperienceConfig config)
    {
        if (string.IsNullOrEmpty(communityId))
            return;

        if (!playersExperience.ContainsKey(communityId))
            playersExperience.Add(communityId, new ExperienceCalculation(config));

        playersExperience[communityId].Loss = true;
    }

    public static void RageQuit(this Dictionary<string, ExperienceCalculation> playersExperience, string? communityId, IExperienceConfig config)
    {
        if (string.IsNullOrEmpty(communityId))
            return;

        if (!playersExperience.ContainsKey(communityId))
            playersExperience.Add(communityId, new ExperienceCalculation(config));

        playersExperience[communityId].RageQuit = true;
    }

    public static void Mvps(this Dictionary<string, ExperienceCalculation> playersExperience, string? communityId, int mvpSiDamage, int mvpCommon, IExperienceConfig config)
    {
        if (string.IsNullOrEmpty(communityId))
            return;

        if (!playersExperience.ContainsKey(communityId))
            playersExperience.Add(communityId, new ExperienceCalculation(config));

        playersExperience[communityId].Mvps += mvpSiDamage;
        playersExperience[communityId].MvpsCommon += mvpCommon;
    }
}