namespace L4D2PlayStats.Core.Modules.Campaigns.Extensions;

public static class CampaignExtensions
{
    public static Campaign? FindUsingMapName(this IEnumerable<Campaign> campaigns, string mapName)
    {
        return campaigns
            .Where(w => w.Maps.Contains(mapName))
            .MaxBy(o => o.Maps.IndexOf(mapName));
    }
}