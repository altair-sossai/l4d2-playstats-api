using L4D2PlayStats.Core.Modules.Matches;

namespace L4D2PlayStats.Core.Modules.Stars.Extensions;

public static class MatchExtensions
{
    public static Dictionary<long, PlayerStars> Stars(this IEnumerable<Match> matches)
    {
        var players = new Dictionary<long, PlayerStars>();

        foreach (var match in matches)
        foreach (var team in match.Teams)
        foreach (var matchPlayer in team.Players)
        {
            if (!long.TryParse(matchPlayer.CommunityId, out var communityId))
                continue;

            if (!players.TryGetValue(communityId, out var player))
            {
                player = new PlayerStars { CommunityId = communityId };
                players.Add(communityId, player);
            }

            player.MvpSiDamage += matchPlayer.MvpSiDamage;
            player.MvpCommon += matchPlayer.MvpCommon;
        }

        return players;
    }
}