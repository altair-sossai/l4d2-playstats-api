using L4D2PlayStats.Core.Modules.Ranking.Enums;
using RankingPlayer = L4D2PlayStats.Core.Modules.Ranking.Player;

namespace L4D2PlayStats.Tests.Modules.Ranking;

[TestClass]
public class PlayerStreakTests
{
    [TestMethod]
    public void Streaks_WithMixedResults_ShouldReturnLongestRunPerType()
    {
        var player = new RankingPlayer
        {
            Results =
            [
                MatchResult.Win, MatchResult.Win, MatchResult.Win,
                MatchResult.Loss,
                MatchResult.Draw, MatchResult.Draw,
                MatchResult.Win, MatchResult.Win,
                MatchResult.Loss, MatchResult.Loss, MatchResult.Loss, MatchResult.Loss,
                MatchResult.Draw
            ]
        };

        Assert.AreEqual(3, player.MaxWinStreak);
        Assert.AreEqual(4, player.MaxLossStreak);
        Assert.AreEqual(2, player.MaxDrawStreak);
    }

    [TestMethod]
    public void Streaks_WithNoResults_ShouldBeZero()
    {
        var player = new RankingPlayer();

        Assert.AreEqual(0, player.MaxWinStreak);
        Assert.AreEqual(0, player.MaxLossStreak);
        Assert.AreEqual(0, player.MaxDrawStreak);
    }

    [TestMethod]
    public void Streaks_WithSingleType_ShouldCountAll()
    {
        var player = new RankingPlayer
        {
            Results = [MatchResult.Win, MatchResult.Win, MatchResult.Win, MatchResult.Win, MatchResult.Win]
        };

        Assert.AreEqual(5, player.MaxWinStreak);
        Assert.AreEqual(0, player.MaxLossStreak);
        Assert.AreEqual(0, player.MaxDrawStreak);
    }
}