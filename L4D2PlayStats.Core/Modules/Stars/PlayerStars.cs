namespace L4D2PlayStats.Core.Modules.Stars;

public class PlayerStars
{
    public long CommunityId { get; init; }
    public int MvpSiDamage { get; set; }
    public int MvpCommon { get; set; }
    public int Stars => MvpSiDamage + MvpCommon;
}