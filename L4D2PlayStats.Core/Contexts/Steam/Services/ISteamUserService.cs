using L4D2PlayStats.Core.Contexts.Steam.Responses;
using L4D2PlayStats.Core.Contexts.Steam.ValueObjects;
using Refit;

namespace L4D2PlayStats.Core.Contexts.Steam.Services;

public interface ISteamUserService
{
	[Get("/ISteamUser/ResolveVanityURL/v0001")]
	Task<ResponseData<ResolveVanityUrl>> ResolveVanityUrlAsync([AliasAs("key")] string key, [AliasAs("vanityurl")] string vanityUrl);

	[Get("/ISteamUser/GetPlayerSummaries/v0002")]
	Task<ResponseData<PlayersInfo>> GetPlayerSummariesAsync([AliasAs("key")] string key, [AliasAs("steamids")] string steamIds);
}