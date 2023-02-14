using L4D2PlayStats.Core.Contexts.Steam.ValueObjects;

namespace L4D2PlayStats.Core.Modules.Steam.Services;

public interface ISteamService
{
	Task<GamesInfo?> GetOwnedGamesAsync(long communityId);
	Task<PlayersInfo?> GetPlayerSummariesAsync(long communityId);
}