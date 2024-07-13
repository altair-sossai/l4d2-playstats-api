﻿using System.Text.Json;
using L4D2PlayStats.Core.Contexts.Steam.SteamUser.Services;
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

    public ISteamUserService SteamUserService => CreateService<ISteamUserService>();

    private static T CreateService<T>()
    {
        return RestService.For<T>(BaseUrl, Settings);
    }
}