using System.Text.Json;
using RankingPlayer = L4D2PlayStats.Core.Modules.Ranking.Player;

namespace L4D2PlayStats.Tests.Modules.Ranking;

[TestClass]
public class PlayerCampaignTests
{
    [TestMethod]
    public void Campaign_ShouldReuseTheSameEntryForTheSameName()
    {
        var player = new RankingPlayer { CommunityId = 76561197960287930 };

        var first = player.Campaign("Dark Carnival");
        var second = player.Campaign("Dark Carnival");
        var other = player.Campaign("The Parish");

        Assert.AreSame(first, second);
        Assert.AreNotSame(first, other);

        first.Wins++;
        second.Loss++;
        other.Draws++;

        Assert.AreEqual(2, player.Campaigns.Count);
        Assert.AreEqual(1, player.Campaigns["Dark Carnival"].Wins);
        Assert.AreEqual(1, player.Campaigns["Dark Carnival"].Loss);
        Assert.AreEqual(1, player.Campaigns["The Parish"].Draws);
    }

    [TestMethod]
    public void Campaigns_ShouldRoundTrip()
    {
        var player = new RankingPlayer { CommunityId = 76561197960287930 };
        var campaign = player.Campaign("Dead Center");
        campaign.Wins = 3;
        campaign.Loss = 2;
        campaign.Draws = 1;
        campaign.RageQuit = 1;

        var json = JsonSerializer.Serialize(player);
        var restored = JsonSerializer.Deserialize<RankingPlayer>(json)!;
        var restoredCampaign = restored.Campaigns["Dead Center"];

        Assert.AreEqual(3, restoredCampaign.Wins);
        Assert.AreEqual(2, restoredCampaign.Loss);
        Assert.AreEqual(1, restoredCampaign.Draws);
        Assert.AreEqual(1, restoredCampaign.RageQuit);
    }

    [TestMethod]
    public void Campaigns_ShouldBeEmptyWhenMissingFromJson()
    {
        var restored = JsonSerializer.Deserialize<RankingPlayer>("{\"CommunityId\":76561197960287930}")!;

        Assert.AreEqual(0, restored.Campaigns.Count);
    }
}
