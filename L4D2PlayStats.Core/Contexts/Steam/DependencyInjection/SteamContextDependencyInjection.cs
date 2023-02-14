using Microsoft.Extensions.DependencyInjection;

namespace L4D2PlayStats.Core.Contexts.Steam.DependencyInjection;

public static class SteamContextDependencyInjection
{
	public static void AddSteamContext(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddScoped(serviceProvider => serviceProvider.GetRequiredService<ISteamContext>().SteamPlayerService);
		serviceCollection.AddScoped(serviceProvider => serviceProvider.GetRequiredService<ISteamContext>().ServerInfoService);
		serviceCollection.AddScoped(serviceProvider => serviceProvider.GetRequiredService<ISteamContext>().SteamUserService);
	}
}