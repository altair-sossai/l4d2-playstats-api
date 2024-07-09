using L4D2PlayStats.Core.Contexts.Steam.SteamUser.Responses;
using L4D2PlayStats.Core.Contexts.Steam.SteamUser.Services;
using L4D2PlayStats.Core.Modules.Ranking.Models.Infrastructure;

namespace L4D2PlayStats.Core.Modules.Ranking.Models;

public class RankingPageModel : PageModel
{
    private const int FirstLineCount = 3;
    private const int SecondLineCount = 5;
    private const int MaxPlayers = 50;

    private readonly List<PlayerModel> _players = [];

    public RankingPageModel(List<Player> players) : base("L4D2PlayStats.Core.Modules.Ranking.Resources.ranking.html")
    {
        Players = players.Take(MaxPlayers).Select(player => new PlayerModel(player)).ToList();
    }

    public List<PlayerModel> FirstLine { get; private set; } = [];
    public List<PlayerModel> SecondLine { get; private set; } = [];

    public List<PlayerModel> Players
    {
        get => _players;
        private init
        {
            FirstLine = value.Take(FirstLineCount).ToList();
            SecondLine = value.Skip(FirstLineCount).Take(SecondLineCount).ToList();
            _players = value;
        }
    }

    public async Task UpdatePlayersAvatarUrlAsync(ISteamUserService steamUserService, string steamApiKey)
    {
        try
        {
            const int take = FirstLineCount + SecondLineCount;

            var steamIds = string.Join(',', Players.Take(take).Select(m => m.CommunityId).Distinct());
            var playerSummaries = await steamUserService.GetPlayerSummariesAsync(steamApiKey, steamIds);

            UpdatePlayersAvatarUrl(playerSummaries);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    private void UpdatePlayersAvatarUrl(GetPlayerSummariesResponse playerSummaries)
    {
        foreach (var playerInfo in playerSummaries.Response?.Players ?? [])
        {
            var player = Players.FirstOrDefault(m => m.CommunityId == playerInfo.SteamId);

            if (player == null || string.IsNullOrEmpty(playerInfo.AvatarFull))
                continue;

            player.AvatarUrl = playerInfo.AvatarFull;
        }
    }

    public class PlayerModel(Player player)
    {
        private string? _avatarUrl = "http://l4d2playstats.blob.core.windows.net/assets/avatar-empty.png";

        public string CommunityId => player.CommunityId.ToString();
        public string? SteamId => player.SteamId;
        public string? Steam3 => player.Steam3;
        public string? ProfileUrl => player.ProfileUrl;

        public string? AvatarUrl
        {
            get => _avatarUrl;
            set => _avatarUrl = value?.Replace("https://", "http://");
        }

        public int Position => player.Position;
        public string? Name => player.Name;
        public int Games => player.Games;
        public int Wins => player.Wins;
        public decimal WinRate => Games == 0 ? 0 : Wins / (decimal)Games;
        public int Mvps => player.Mvps;
        public int Loss => player.Loss;
    }
}