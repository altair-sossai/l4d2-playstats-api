﻿using L4D2PlayStats.Core.Contexts.Steam.SteamUser.Responses;
using Refit;

namespace L4D2PlayStats.Core.Contexts.Steam.SteamUser.Services;

public interface ISteamUserService
{
    [Get("/ISteamUser/GetPlayerSummaries/v0002")]
    Task<GetPlayerSummariesResponse> GetPlayerSummariesAsync([AliasAs("key")] string key, [AliasAs("steamids")] string steamIds);
}