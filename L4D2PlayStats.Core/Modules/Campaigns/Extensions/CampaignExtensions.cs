namespace L4D2PlayStats.Core.Modules.Campaigns.Extensions;

public static class CampaignExtensions
{
    public static Dictionary<string, Campaign> Maps(this IEnumerable<Campaign> campaigns)
    {
        var dictionary = new Dictionary<string, Campaign>();

        foreach (var campaign in campaigns)
            foreach (var map in campaign.Maps)
                dictionary.Add(map, campaign);

        return dictionary;
    }

    public static bool SequentialMaps(this Campaign campaign, string current, string next)
    {
        var currentIndexOf = campaign.Maps.IndexOf(current);
        var nextIndexOf = campaign.Maps.IndexOf(next);

        return currentIndexOf != -1
               && nextIndexOf != -1
               && currentIndexOf == nextIndexOf - 1;
    }
}