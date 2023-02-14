using L4D2PlayStats.Core.Contexts.Steam.Responses;
using L4D2PlayStats.Core.Contexts.Steam.ValueObjects;
using Refit;

namespace L4D2PlayStats.Core.Contexts.Steam.Services;

public interface ISteamPlayerService
{
	[Get("/IPlayerService/GetOwnedGames/v0001")]
	Task<ResponseData<GamesInfo>> GetOwnedGamesAsync([AliasAs("key")] string key, [AliasAs("steamid")] string steamId);
}