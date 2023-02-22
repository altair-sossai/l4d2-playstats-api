namespace L4D2PlayStats.Core.Modules.Matches.Structures;

public class MatchPoints
{
    public string CommunityId { get; set; }
    public string Name { get; set; }
    public decimal Points { get; set; }

    public Players.Player ToPlayer()
    {
        return new Players.Player
        {
            CommunityId = long.Parse(CommunityId),
            Name = Name
        };
    }
}