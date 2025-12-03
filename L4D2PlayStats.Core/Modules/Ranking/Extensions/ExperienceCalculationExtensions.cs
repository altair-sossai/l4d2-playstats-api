using L4D2PlayStats.Core.Modules.Ranking.Configs;
using L4D2PlayStats.Core.Modules.Ranking.Structures;

namespace L4D2PlayStats.Core.Modules.Ranking.Extensions;

public static class ExperienceCalculationExtensions
{
    extension(Dictionary<string, ExperienceCalculation> playersExperience)
    {
        public void Win(string? communityId, IExperienceConfig config)
        {
            if (string.IsNullOrEmpty(communityId))
                return;

            if (!playersExperience.ContainsKey(communityId))
                playersExperience.Add(communityId, new ExperienceCalculation(config));

            playersExperience[communityId].Win = true;
        }

        public void Loss(string? communityId, IExperienceConfig config)
        {
            if (string.IsNullOrEmpty(communityId))
                return;

            if (!playersExperience.ContainsKey(communityId))
                playersExperience.Add(communityId, new ExperienceCalculation(config));

            playersExperience[communityId].Loss = true;
        }

        public void RageQuit(string? communityId, IExperienceConfig config)
        {
            if (string.IsNullOrEmpty(communityId))
                return;

            if (!playersExperience.ContainsKey(communityId))
                playersExperience.Add(communityId, new ExperienceCalculation(config));

            playersExperience[communityId].RageQuit = true;
        }

        public void Mvps(string? communityId, int mvpSiDamage, int mvpCommon, IExperienceConfig config)
        {
            if (string.IsNullOrEmpty(communityId))
                return;

            if (!playersExperience.ContainsKey(communityId))
                playersExperience.Add(communityId, new ExperienceCalculation(config));

            playersExperience[communityId].Mvps += mvpSiDamage;
            playersExperience[communityId].MvpsCommon += mvpCommon;
        }
    }
}