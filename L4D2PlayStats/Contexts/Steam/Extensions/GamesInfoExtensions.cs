using L4D2PlayStats.Contexts.Steam.ValueObjects;

namespace L4D2PlayStats.Contexts.Steam.Extensions;

public static class GamesInfoExtensions
{
    public static GameInfo? Left4Dead2(this GamesInfo? gamesInfo)
    {
        return gamesInfo?.Games?.FirstOrDefault(f => f?.AppId == 550);
    }
}