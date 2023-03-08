namespace L4D2PlayStats.Core.Modules.Mix.Results;

public class MixResult
{
    private static readonly int[] SurvivorsTeam = { 1, 4, 6, 8 };
    private static readonly int[] InfectedsTeam = { 2, 3, 5, 7 };


    public MixResult(IReadOnlyList<string> availables, IReadOnlyDictionary<string, Ranking.Player> players)
    {
        foreach (var survivor in SurvivorsTeam)
            Survivors.Add(players[availables[survivor - 1]]);

        foreach (var infected in InfectedsTeam)
            Infecteds.Add(players[availables[infected - 1]]);
    }

    public List<Ranking.Player> Survivors { get; } = new();
    public List<Ranking.Player> Infecteds { get; } = new();
}