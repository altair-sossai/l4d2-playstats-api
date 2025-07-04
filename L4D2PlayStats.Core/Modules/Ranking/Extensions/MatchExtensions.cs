using L4D2PlayStats.Core.Modules.Matches;
using L4D2PlayStats.Core.Modules.Ranking.Configs;
using L4D2PlayStats.Core.Modules.Ranking.Structures;

namespace L4D2PlayStats.Core.Modules.Ranking.Extensions;

public static class MatchExtensions
{
    public static IEnumerable<Player> Ranking(this Match match, Dictionary<string, int> punishments, IExperienceConfig config)
    {
        var matches = new[] { match };

        return matches.Ranking(punishments, config);
    }

    public static IEnumerable<Player> Ranking(this IEnumerable<Match> matches, Dictionary<string, int> punishments, IExperienceConfig config)
    {
        var players = new Dictionary<string, Player>();
        var previousExperience = new Dictionary<string, decimal>();

        foreach (var match in matches.Reverse())
        {
            var playersExperience = new Dictionary<string, ExperienceCalculation>();

            foreach (var playerName in match.Winners())
            {
                var player = players.TryAdd(playerName);
                if (player != null)
                    player.Wins++;

                playersExperience.Win(playerName.CommunityId, config);
            }

            foreach (var playerName in match.Losers())
            {
                var player = players.TryAdd(playerName);
                if (player != null)
                    player.Loss++;

                playersExperience.Loss(playerName.CommunityId, config);
            }

            foreach (var statsPlayer in match.RageQuit())
            {
                var player = players.TryAdd(statsPlayer);
                if (player != null)
                {
                    player.Loss++;
                    player.RageQuit++;
                }

                playersExperience.RageQuit(statsPlayer.CommunityId, config);
            }

            previousExperience.Clear();

            foreach (var half in match.MapsStatistics.SelectMany(map => map.Statistic?.Halves ?? []))
            {
                foreach (var matchPlayer in half.Players
                             .Where(matchPlayer => !string.IsNullOrEmpty(matchPlayer.CommunityId)
                                                   && players.ContainsKey(matchPlayer.CommunityId)
                                                   && !previousExperience.ContainsKey(matchPlayer.CommunityId)))
                {
                    if (string.IsNullOrEmpty(matchPlayer.CommunityId))
                        continue;

                    previousExperience.Add(matchPlayer.CommunityId, players[matchPlayer.CommunityId].Experience);
                }

                foreach (var matchPlayer in half.InfectedPlayers
                             .Where(matchPlayer => !string.IsNullOrEmpty(matchPlayer.CommunityId)
                                                   && players.ContainsKey(matchPlayer.CommunityId)
                                                   && !previousExperience.ContainsKey(matchPlayer.CommunityId)))
                {
                    if (string.IsNullOrEmpty(matchPlayer.CommunityId))
                        continue;

                    previousExperience.Add(matchPlayer.CommunityId, players[matchPlayer.CommunityId].Experience);
                }
            }

            foreach (var team in match.Teams)
            foreach (var matchPlayer in team.Players)
            {
                var player = players.TryAdd(matchPlayer);
                if (player == null)
                    continue;

                player.AppendInfo(matchPlayer);

                playersExperience.Mvps(matchPlayer.CommunityId, matchPlayer.MvpSiDamage, matchPlayer.MvpCommon, config);
            }

            foreach (var (communityId, experienceCalculation) in playersExperience)
            {
                if (!players.TryGetValue(communityId, out var player))
                    continue;

                player.Experience += experienceCalculation.Experience;
            }
        }

        foreach (var (key, value) in punishments)
        {
            if (!players.TryGetValue(key, out var player))
                continue;

            player.Punishment = value;
            player.Experience -= value;
        }

        foreach (var (communityId, experience) in previousExperience)
        {
            if (!players.TryGetValue(communityId, out var player))
                continue;

            player.PreviousExperience = experience;
        }

        return players.Values.RankPlayers();
    }

    private static IEnumerable<PlayerName> Winners(this Match match)
    {
        var firstRoundPlayers = match.FirstRoundPlayers?.Select(p => p.CommunityId).ToHashSet();
        if (firstRoundPlayers == null)
            yield break;

        var lastMap = match.MapsStatistics.Select(m => m.Statistic).FirstOrDefault();

        if (lastMap?.Scoring?.TeamA == null
            || lastMap.Scoring?.TeamB == null
            || lastMap.Scoring.TeamA.Score == lastMap.Scoring.TeamB.Score)
            yield break;

        var winners = lastMap.Scoring.TeamA.Score > lastMap.Scoring.TeamB.Score ? lastMap.TeamA : lastMap.TeamB;

        foreach (var playerName in winners.Where(w => firstRoundPlayers.Contains(w.CommunityId)))
            yield return playerName;
    }

    private static IEnumerable<PlayerName> Losers(this Match match)
    {
        var firstRoundPlayers = match.FirstRoundPlayers?.Select(p => p.CommunityId).ToHashSet();
        if (firstRoundPlayers == null)
            yield break;

        var lastMap = match.MapsStatistics.Select(m => m.Statistic).FirstOrDefault();

        if (lastMap?.Scoring?.TeamA == null
            || lastMap.Scoring?.TeamB == null
            || lastMap.Scoring.TeamA.Score == lastMap.Scoring.TeamB.Score)
            yield break;

        var losers = lastMap.Scoring.TeamA.Score > lastMap.Scoring.TeamB.Score ? lastMap.TeamB : lastMap.TeamA;

        foreach (var playerName in losers.Where(w => firstRoundPlayers.Contains(w.CommunityId)))
            yield return playerName;
    }

    private static IEnumerable<L4D2PlayStats.Player> RageQuit(this Match match)
    {
        var firstRoundPlayers = match.FirstRoundPlayers?.ToList();
        if (firstRoundPlayers == null)
            yield break;

        var lastRoundPlayers = match.LastRoundPlayers?.ToList();
        if (lastRoundPlayers == null)
            yield break;

        foreach (var firstRoundPlayer in firstRoundPlayers.Where(frp => lastRoundPlayers.All(lrp => lrp.CommunityId != frp.CommunityId)))
            yield return firstRoundPlayer;
    }
}