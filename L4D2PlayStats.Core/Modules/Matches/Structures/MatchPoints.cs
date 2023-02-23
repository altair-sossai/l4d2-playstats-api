namespace L4D2PlayStats.Core.Modules.Matches.Structures;

public class MatchPoints
{
    public string CommunityId { get; init; } = default!;
    public string Name { get; init; } = default!;
    public decimal Points { get; init; }

    public Players.Player ToPlayer()
    {
        return new Players.Player
        {
            CommunityId = long.Parse(CommunityId),
            Name = Name
        };
    }
}