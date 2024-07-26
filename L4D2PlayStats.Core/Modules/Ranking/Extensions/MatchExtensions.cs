﻿using L4D2PlayStats.Core.Modules.Matches;
using L4D2PlayStats.Core.Modules.Ranking.Configs;
using L4D2PlayStats.Core.Modules.Ranking.Structures;

namespace L4D2PlayStats.Core.Modules.Ranking.Extensions;

public static class MatchExtensions
{
    public static IEnumerable<Player> Ranking(this Match match, IExperienceConfig config)
    {
        var matches = new[] { match };

        return matches.Ranking(config);
    }

    public static IEnumerable<Player> Ranking(this IEnumerable<Match> matches, IExperienceConfig config)
    {
        var players = new Dictionary<string, Player>();
        var previousExperience = new Dictionary<string, decimal>();

        foreach (var match in matches.Reverse())
        {
            previousExperience.Clear();

            foreach (var team in match.Teams)
            foreach (var matchPlayer in team.Players)
            {
                if (string.IsNullOrEmpty(matchPlayer.CommunityId)
                    || !players.ContainsKey(matchPlayer.CommunityId)
                    || previousExperience.ContainsKey(matchPlayer.CommunityId))
                    continue;

                previousExperience.Add(matchPlayer.CommunityId, players[matchPlayer.CommunityId].Experience);
            }

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

            foreach (var team in match.Teams)
            foreach (var matchPlayer in team.Players)
            {
                var player = players.TryAdd(matchPlayer);
                if (player == null)
                    continue;

                player.Games++;
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
        var lastMap = match.Maps.Select(m => m.Statistic).FirstOrDefault();

        if (lastMap?.Scoring?.TeamA == null
            || lastMap.Scoring?.TeamB == null
            || lastMap.Scoring.TeamA.Score == lastMap.Scoring.TeamB.Score)
            yield break;

        var winners = lastMap.Scoring.TeamA.Score > lastMap.Scoring.TeamB.Score ? lastMap.TeamA : lastMap.TeamB;

        foreach (var playerName in winners)
            yield return playerName;
    }

    private static IEnumerable<PlayerName> Losers(this Match match)
    {
        var lastMap = match.Maps.Select(m => m.Statistic).FirstOrDefault();

        if (lastMap?.Scoring?.TeamA == null
            || lastMap.Scoring?.TeamB == null
            || lastMap.Scoring.TeamA.Score == lastMap.Scoring.TeamB.Score)
            yield break;

        var losers = lastMap.Scoring.TeamA.Score > lastMap.Scoring.TeamB.Score ? lastMap.TeamB : lastMap.TeamA;

        foreach (var playerName in losers)
            yield return playerName;
    }
}