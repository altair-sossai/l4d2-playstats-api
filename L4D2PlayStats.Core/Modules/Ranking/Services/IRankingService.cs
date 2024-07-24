namespace L4D2PlayStats.Core.Modules.Ranking.Services;

public interface IRankingService
{
    Task<List<Player>> RankingAsync(string serverId, int count);
}