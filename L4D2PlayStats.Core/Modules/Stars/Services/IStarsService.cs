namespace L4D2PlayStats.Core.Modules.Stars.Services;

public interface IStarsService
{
    Task<List<PlayerStars>> StarsAsync(string serverId);
    Task<PlayerStars> StarsAsync(string serverId, long communityId);
    Task SaveClosedStarsAsync(string serverId);
}