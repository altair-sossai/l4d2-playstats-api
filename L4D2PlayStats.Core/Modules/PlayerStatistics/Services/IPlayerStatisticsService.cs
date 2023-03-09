namespace L4D2PlayStats.Core.Modules.PlayerStatistics.Services;

public interface IPlayerStatisticsService
{
    Task<List<Player>> PlayerStatisticsAsync(string serverId);
}