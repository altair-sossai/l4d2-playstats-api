namespace L4D2PlayStats.Core.Modules.Statistics.Extensions.L4D2PlayStats;

public static class PlayerNameExtensions
{
    public static HashSet<string> ToHashSet(this IEnumerable<PlayerName> playerNames)
    {
        return playerNames.Select(s => s.CommunityId).Cast<string>().ToHashSet();
    }
}