using L4D2PlayStats.Core.Contexts.Steam.SteamUser.Services;

namespace L4D2PlayStats.Core.Contexts.Steam;

public interface ISteamContext
{
    ISteamUserService SteamUserService { get; }
}