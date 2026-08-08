namespace L4D2PlayStats.Core.Modules.Ranking.Structures;

public class PlayerRelation(long communityId)
{
    public long CommunityId { get; } = communityId;

    public int TogetherWins { get; set; }
    public int TogetherLosses { get; set; }
    public int TogetherGames => TogetherWins + TogetherLosses;

    public int AgainstWins { get; set; }
    public int AgainstLosses { get; set; }
    public int AgainstGames => AgainstWins + AgainstLosses;
}