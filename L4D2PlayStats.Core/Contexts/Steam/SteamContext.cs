using System.Text.Json;
using L4D2PlayStats.Core.Contexts.Steam.Services;
using Microsoft.Extensions.Configuration;
using Refit;

namespace L4D2PlayStats.Core.Contexts.Steam;

public class SteamContext : ISteamContext
{
    private const string BaseUrl = "https://api.steampowered.com";

    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    private static readonly RefitSettings Settings = new()
    {
        ContentSerializer = new SystemTextJsonContentSerializer(Options)
    };

    private readonly IConfiguration _configuration;

    public SteamContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string SteamApiKey => _configuration.GetValue<string>(nameof(SteamApiKey))!;

    public ISteamPlayerService SteamPlayerService => CreateService<ISteamPlayerService>();
    public IServerInfoService ServerInfoService => CreateService<IServerInfoService>();
    public ISteamUserService SteamUserService => CreateService<ISteamUserService>();

    private static T CreateService<T>()
    {
        return RestService.For<T>(BaseUrl, Settings);
    }
}