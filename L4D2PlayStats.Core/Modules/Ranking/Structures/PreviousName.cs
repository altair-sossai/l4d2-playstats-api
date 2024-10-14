namespace L4D2PlayStats.Core.Modules.Ranking.Structures;

public class PreviousName(string name)
{
    public string Name { get; } = name;
    public int TimesUsed { get; set; }
}