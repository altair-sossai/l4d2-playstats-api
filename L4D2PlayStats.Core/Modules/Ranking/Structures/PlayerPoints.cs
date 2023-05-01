using L4D2PlayStats.Steam;

namespace L4D2PlayStats.Core.Modules.Ranking.Structures;

public class PlayerPoints : SteamUser
{
    private const int WinPoints = 4;
    private const int LostPoints = -4;
    private const int TiedPoints = 2;
    private const int RageQuitPoints = -6;

    private PlayerPoints(PlayerName playerName)
    {
        SteamId = playerName.SteamId;
        Name = playerName.Name;
    }

    public string? Name { get; set; }
    public int Points { get; private init; }
    public bool Winner { get; private init; }
    public bool Loser { get; private init; }
    public bool Draw { get; private init; }
    public bool Rage { get; private init; }

    public static PlayerPoints Win(PlayerName playerName)
    {
        return new PlayerPoints(playerName)
        {
            Points = WinPoints,
            Winner = true
        };
    }

    public static PlayerPoints Lost(PlayerName playerName)
    {
        return new PlayerPoints(playerName)
        {
            Points = LostPoints,
            Loser = true
        };
    }

    public static PlayerPoints Tied(PlayerName playerName)
    {
        return new PlayerPoints(playerName)
        {
            Points = TiedPoints,
            Draw = true
        };
    }

    public static PlayerPoints RageQuit(PlayerName playerName)
    {
        return new PlayerPoints(playerName)
        {
            Points = RageQuitPoints,
            Rage = true
        };
    }
}