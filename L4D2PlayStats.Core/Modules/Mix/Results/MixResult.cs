namespace L4D2PlayStats.Core.Modules.Mix.Results;

public class MixResult
{
    private static readonly int[] SurvivorsTeam = { 1, 4, 6, 8 };
    private static readonly int[] InfectedsTeam = { 2, 3, 5, 7 };


    public MixResult(IReadOnlyList<string> availables, IReadOnlyDictionary<string, Ranking.Player> players)
    {
        foreach (var survivor in SurvivorsTeam)
            Survivors.Add(new Player(players[availables[survivor - 1]]));

        foreach (var infected in InfectedsTeam)
            Infecteds.Add(new Player(players[availables[infected - 1]]));
    }

    public List<Player> Survivors { get; } = new();
    public List<Player> Infecteds { get; } = new();

    public class Player
    {
        private readonly Ranking.Player _player;

        public Player(Ranking.Player player)
        {
            _player = player;
        }

        public int Position => _player.Position;
        public string CommunityId => _player.CommunityId.ToString();
        public string? Name => _player.Name;
    }
}