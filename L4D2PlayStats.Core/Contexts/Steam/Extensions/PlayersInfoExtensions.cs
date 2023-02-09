using L4D2PlayStats.Core.Contexts.Steam.ValueObjects;

namespace L4D2PlayStats.Core.Contexts.Steam.Extensions;

public static class PlayersInfoExtensions
{
    public static PlayerInfo? FirstPlayerOrDefault(this PlayersInfo? playersInfo)
    {
        return playersInfo?.Players?.FirstOrDefault();
    }
}