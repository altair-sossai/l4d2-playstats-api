namespace L4D2PlayStats.Core.Modules.Ranking.Models;

public class RankingPageModel
{
    private readonly List<PlayerModel> _players = [];

    public RankingPageModel(List<Player> players)
    {
        Players = players.Select(player => new PlayerModel(player)).ToList();
    }

    public List<PlayerModel> FirstLine { get; private set; } = [];
    public List<PlayerModel> SecondLine { get; private set; } = [];

    public List<PlayerModel> Players
    {
        get => _players;
        private init
        {
            FirstLine = value.Take(3).ToList();
            SecondLine = value.Skip(3).Take(5).ToList();
            _players = value;
        }
    }

    public class PlayerModel(Player player)
    {
        public string CommunityId => player.CommunityId.ToString();
        public string? SteamId => player.SteamId;
        public string? Steam3 => player.Steam3;
        public string? ProfileUrl => player.ProfileUrl;
        public string? AvatarUrl { get; set; } = "http://l4d2playstats.blob.core.windows.net/assets/avatar-empty.png";
        public int Position => player.Position;
        public string? Name => player.Name;
        public int Wins => player.Wins;
        public int Mvps => player.Mvps;
        public int Loss => player.Loss;
    }
}