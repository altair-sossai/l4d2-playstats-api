using System.Globalization;
using L4D2PlayStats.Core.Contexts.Steam.SteamUser.Responses;
using L4D2PlayStats.Core.Contexts.Steam.SteamUser.Services;
using L4D2PlayStats.Core.Infrastructure.Extensions;
using L4D2PlayStats.Core.Infrastructure.Helpers;
using NUglify;
using Stubble.Core;
using Stubble.Core.Builders;

namespace L4D2PlayStats.Core.Modules.Ranking.Models;

public class RankingPageModel
{
    private const int FirstLineCount = 3;
    private const int SecondLineCount = 5;
    private const int MaxPlayers = 50;

    private static string? _template;
    private static StubbleVisitorRenderer? _stuble;

    private readonly List<PlayerModel> _players = [];

    public RankingPageModel(List<Player> players)
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

    private static StubbleVisitorRenderer Stuble
    {
        get
        {
            if (_stuble != null)
                return _stuble;

            var stubbleBuilder = new StubbleBuilder();

            return _stuble = stubbleBuilder.Build();
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
            var model = Players.FirstOrDefault(m => m.CommunityId == playerInfo.SteamId);

            if (model == null || string.IsNullOrEmpty(playerInfo.AvatarFull))
                continue;

            model.AvatarUrl = playerInfo.AvatarFull;
        }
    }

    public async Task<Stream> RenderAsync()
    {
        _template ??= await EmbeddedResourceHelper.LoadEmbeddedResourceAsync("L4D2PlayStats.Core.Modules.Ranking.Resources.ranking.html");

        var html = await Stuble.RenderAsync(_template, this);
        var uglifyResult = Uglify.Html(html);

        var memoryStream = new MemoryStream();
        var streamWriter = new StreamWriter(memoryStream);

        await streamWriter.WriteAsync(uglifyResult.Code);
        await streamWriter.FlushAsync();

        memoryStream.Position = 0;

        return memoryStream;
    }

    public class PlayerModel(Player player)
    {
        private static readonly CultureInfo CultureInfo = new("en-us");
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
        public string? Name => player.Name?.Truncate(30);
        public string? ShortName => player.Name?.Truncate(12);
        public int Games => player.Games;
        public int Wins => player.Wins;
        public decimal WinRate => Games == 0 ? 0 : Wins / (decimal)Games;
        public string WinRateFormated => WinRate.ToString("P0", CultureInfo);
        public int Mvps => player.Mvps;
        public int Loss => player.Loss;
    }
}