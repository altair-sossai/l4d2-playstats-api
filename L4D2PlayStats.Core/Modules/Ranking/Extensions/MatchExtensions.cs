using L4D2PlayStats.Core.Modules.Matches;
using L4D2PlayStats.Core.Modules.Ranking.Configs;
using L4D2PlayStats.Core.Modules.Ranking.Structures;

namespace L4D2PlayStats.Core.Modules.Ranking.Extensions;

public static class MatchExtensions
{
    public static IEnumerable<Player> Ranking(this IEnumerable<Match> matches, Dictionary<string, int> punishments, IExperienceConfig config)
    {
        var players = new Dictionary<string, Player>();
        var previousExperience = new Dictionary<string, decimal>();

        foreach (var match in matches.Reverse())
        {
            var playersExperience = new Dictionary<string, ExperienceCalculation>();

            var winners = match.Winners().ToList();
            var losers = match.Losers().ToList();

            foreach (var playerName in winners)
            {
                var player = players.TryAdd(playerName);
                if (player != null)
                    player.Wins++;

                playersExperience.Win(playerName.CommunityId, config);
            }

            foreach (var playerName in losers)
            {
                var player = players.TryAdd(playerName);
                if (player != null)
                    player.Loss++;

                playersExperience.Loss(playerName.CommunityId, config);
            }

            players.BuildRelations(winners, losers);

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

        foreach (var (communityId, experience) in previousExperience)
        {
            if (!players.TryGetValue(communityId, out var player))
                continue;

            player.PreviousExperience = experience;
        }

        foreach (var (key, punishment) in punishments)
        {
            if (!players.TryGetValue(key, out var player))
                continue;

            player.Punishment = punishment;
            player.Experience -= punishment;
            player.PreviousExperience -= punishment;
        }

        return players.Values.RankPlayers();
    }

    extension(Dictionary<string, Player> players)
    {
        private void BuildRelations(IReadOnlyList<PlayerName> winners, IReadOnlyList<PlayerName> losers)
        {
            players.AccumulateTogether(winners, true);
            players.AccumulateTogether(losers, false);

            players.AccumulateAgainst(winners, losers, true);
            players.AccumulateAgainst(losers, winners, false);
        }

        private void AccumulateTogether(IReadOnlyList<PlayerName> teammates, bool won)
        {
            foreach (var teammate in teammates)
            {
                if (string.IsNullOrEmpty(teammate.CommunityId) || !players.TryGetValue(teammate.CommunityId, out var player))
                    continue;

                foreach (var other in teammates)
                {
                    if (string.IsNullOrEmpty(other.CommunityId) || other.CommunityId == teammate.CommunityId)
                        continue;

                    var relation = player.Relation(long.Parse(other.CommunityId));

                    if (won)
                        relation.TogetherWins++;
                    else
                        relation.TogetherLosses++;
                }
            }
        }

        private void AccumulateAgainst(IReadOnlyList<PlayerName> side, IReadOnlyList<PlayerName> opponents, bool won)
        {
            foreach (var current in side)
            {
                if (string.IsNullOrEmpty(current.CommunityId) || !players.TryGetValue(current.CommunityId, out var player))
                    continue;

                foreach (var opponent in opponents)
                {
                    if (string.IsNullOrEmpty(opponent.CommunityId))
                        continue;

                    var relation = player.Relation(long.Parse(opponent.CommunityId));

                    if (won)
                        relation.AgainstWins++;
                    else
                        relation.AgainstLosses++;
                }
            }
        }
    }

    extension(Match match)
    {
        public IEnumerable<Player> Ranking(Dictionary<string, int> punishments, IExperienceConfig config)
        {
            var matches = new[] { match };

            return matches.Ranking(punishments, config);
        }

        private IEnumerable<PlayerName> Winners()
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

        private IEnumerable<PlayerName> Losers()
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

        private IEnumerable<L4D2PlayStats.Player> RageQuit()
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
}