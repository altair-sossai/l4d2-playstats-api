using L4D2PlayStats.Core.Contexts.Steam.SteamUser.Responses;
using L4D2PlayStats.Core.Contexts.Steam.SteamUser.Services;
using L4D2PlayStats.Core.Modules.Matches;
using L4D2PlayStats.Core.Modules.Ranking.Models.Infrastructure;

namespace L4D2PlayStats.Core.Modules.Ranking.Models;

public class LastMatchPageModel : PageModel
{
    public LastMatchPageModel(Match match) : base("L4D2PlayStats.Core.Modules.Ranking.Resources.last-match.html")
    {
        Match = match;

        Maps = Match.Maps
            .Select((statistics, index) => new MapModel(statistics, Match.Maps.Count - index))
            .OrderBy(o => o.Round)
            .ToList();

        PlayersTeamA = TeamA.Players
            .OrderByDescending(o => o.MvpSiDamage)
            .ThenByDescending(tb => tb.MvpCommon)
            .ThenBy(tb => tb.LvpFfGiven)
            .Select(player => new PlayerModel(player))
            .ToList();

        PlayersTeamB = TeamB.Players
            .OrderByDescending(o => o.MvpSiDamage)
            .ThenByDescending(tb => tb.MvpCommon)
            .ThenBy(tb => tb.LvpFfGiven)
            .Select(player => new PlayerModel(player))
            .ToList();

        var chunkedA = PlayersTeamA.Chunk(2).ToList();
        var chunkedB = PlayersTeamB.Chunk(2).ToList();

        for (var i = 0; i < Math.Max(chunkedA.Count, chunkedB.Count); i++)
        {
            var grouped = new PlayersGroupedModel();

            if (i < chunkedA.Count)
                grouped.PlayersA.AddRange(chunkedA[i]);

            if (i < chunkedB.Count)
                grouped.PlayersB.AddRange(chunkedB[i]);

            PlayersGrouped.Add(grouped);
        }
    }

    public Match Match { get; }
    public List<MapModel> Maps { get; }

    public List<PlayerModel> PlayersTeamA { get; }
    public List<PlayerModel> PlayersTeamB { get; }

    public List<PlayersGroupedModel> PlayersGrouped { get; } = [];

    public Match.Team TeamA => Match.Teams[0];
    public Match.Team TeamB => Match.Teams[1];

    public int TeamAScore => TeamA.Score;
    public int TeamBScore => TeamB.Score;

    public bool Draw => TeamAScore == TeamBScore;
    public bool TeamAWon => TeamAScore > TeamBScore;
    public bool TeamBWon => TeamBScore > TeamAScore;

    public int VsRowSpan => PlayersGrouped.Count + 1;

    public string TeamAScoreboardTitle => Draw ? "Draw" : TeamAWon ? "Winner" : "Loser";
    public string TeamBScoreboardTitle => Draw ? "Draw" : TeamBWon ? "Winner" : "Loser";

    public string TeamAScoreboardClass => Draw ? "draw" : TeamAWon ? "winner" : "loser";
    public string TeamBScoreboardClass => Draw ? "draw" : TeamBWon ? "winner" : "loser";

    public async Task UpdatePlayersAvatarUrlAsync(ISteamUserService steamUserService, string steamApiKey)
    {
        try
        {
            var steamIds = string.Join(',', Match.Teams.SelectMany(sm => sm.Players).Select(m => m.CommunityId).Distinct());
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
            var player = PlayersTeamA.FirstOrDefault(m => m.CommunityId == playerInfo.SteamId)
                         ?? PlayersTeamB.FirstOrDefault(m => m.CommunityId == playerInfo.SteamId);

            if (player == null || string.IsNullOrEmpty(playerInfo.AvatarFull))
                continue;

            player.AvatarUrl = playerInfo.AvatarFull;
        }
    }

    public class PlayerModel(Match.Player player)
    {
        private string? _avatarUrl = "http://l4d2playstats.blob.core.windows.net/assets/avatar-empty.png";

        public string? CommunityId => player.CommunityId;
        public string? SteamId => player.SteamId;
        public string? Steam3 => player.Steam3;
        public string? ProfileUrl => player.ProfileUrl;

        public string? AvatarUrl
        {
            get => _avatarUrl;
            set => _avatarUrl = value?.Replace("https://", "http://");
        }

        public string? Name => player.Name;
        public int Mvps => player.MvpSiDamage;
        public int MvpCommons => player.MvpCommon;
    }

    public class PlayersGroupedModel
    {
        public List<PlayerModel> PlayersA { get; } = [];
        public List<PlayerModel> PlayersB { get; } = [];
    }

    public class MapModel(Statistics.Statistics statistics, int round)
    {
        public int Round => round;
        public string? Name => statistics.MapName;
        public TimeSpan? Elapsed => statistics.Statistic?.MapElapsed;
        public int? TeamAScore => statistics.Statistic?.Scoring?.TeamA?.Score;
        public int? TeamBScore => statistics.Statistic?.Scoring?.TeamB?.Score;
        public int? ScoreDifference => statistics.ScoreDifference;
    }
}