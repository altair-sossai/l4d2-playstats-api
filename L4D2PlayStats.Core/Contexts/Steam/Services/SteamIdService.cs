using L4D2PlayStats.Core.Contexts.Steam.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Core.Contexts.Steam.Services;

public class SteamIdService : ISteamIdService
{
	private readonly IMemoryCache _memoryCache;
	private readonly ISteamContext _steamContext;
	private readonly ISteamUserService _steamUserService;

	public SteamIdService(IMemoryCache memoryCache, ISteamContext steamContext, ISteamUserService steamUserService)
	{
		_memoryCache = memoryCache;
		_steamContext = steamContext;
		_steamUserService = steamUserService;
	}

	public async Task<long?> ResolveSteamIdAsync(string? customUrl)
	{
		if (string.IsNullOrEmpty(customUrl))
			return await Task.FromResult((long?)null);

		return await _memoryCache.GetOrCreateAsync($"ResolveVanityURL_{customUrl}", async factory =>
		{
			factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

			var responseData = await _steamUserService.ResolveVanityUrlAsync(_steamContext.SteamApiKey, customUrl);

			return responseData.Response?.SteamId();
		});
	}
}