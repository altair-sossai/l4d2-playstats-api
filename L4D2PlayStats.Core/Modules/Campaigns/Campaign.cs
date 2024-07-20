namespace L4D2PlayStats.Core.Modules.Campaigns;

public class Campaign
{
    public string? Name { get; set; }
    public List<string> Maps { get; set; } = new();

    public bool SequentialMaps(string current, string next)
    {
        var currentIndexOf = Maps.IndexOf(current);
        if (currentIndexOf == -1)
            return false;

        var nextIndexOf = Maps.IndexOf(next);
        if (nextIndexOf == -1)
            return false;

        return currentIndexOf == nextIndexOf - 1;
    }
}