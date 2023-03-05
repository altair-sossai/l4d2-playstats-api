namespace L4D2PlayStats.Core.Modules.Mix.Commands;

public class MixCommand
{
    public string? Player1 { get; set; }
    public string? Player2 { get; set; }
    public string? Player3 { get; set; }
    public string? Player4 { get; set; }
    public string? Player5 { get; set; }
    public string? Player6 { get; set; }
    public string? Player7 { get; set; }
    public string? Player8 { get; set; }

    public IEnumerable<string> All
    {
        get
        {
            if (!string.IsNullOrEmpty(Player1))
                yield return Player1;

            if (!string.IsNullOrEmpty(Player2))
                yield return Player2;

            if (!string.IsNullOrEmpty(Player3))
                yield return Player3;

            if (!string.IsNullOrEmpty(Player4))
                yield return Player4;

            if (!string.IsNullOrEmpty(Player5))
                yield return Player5;

            if (!string.IsNullOrEmpty(Player6))
                yield return Player6;

            if (!string.IsNullOrEmpty(Player7))
                yield return Player7;

            if (!string.IsNullOrEmpty(Player8))
                yield return Player8;
        }
    }
}