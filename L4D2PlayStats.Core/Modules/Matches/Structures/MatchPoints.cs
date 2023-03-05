namespace L4D2PlayStats.Core.Modules.Matches.Structures;

public class MatchPoints
{
    public string CommunityId { get; init; } = default!;
    public string Name { get; init; } = default!;
    public decimal Points { get; init; }

    public Ranking.Player ToPlayer()
    {
        return new Ranking.Player
        {
            CommunityId = long.Parse(CommunityId),
            Name = Name
        };
    }
}