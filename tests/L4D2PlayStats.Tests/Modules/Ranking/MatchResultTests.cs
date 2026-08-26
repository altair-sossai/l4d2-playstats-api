using System.Text.Json;
using L4D2PlayStats.Core.Modules.Ranking.Enums;
using RankingPlayer = L4D2PlayStats.Core.Modules.Ranking.Player;

namespace L4D2PlayStats.Tests.Modules.Ranking;

[TestClass]
public class MatchResultTests
{
    [TestMethod]
    [DataRow(MatchResult.Win, "\"W\"")]
    [DataRow(MatchResult.Loss, "\"L\"")]
    [DataRow(MatchResult.Draw, "\"D\"")]
    public void Serialize_ShouldUseSingleLetterCode(MatchResult result, string expectedJson)
    {
        var json = JsonSerializer.Serialize(result);

        Assert.AreEqual(expectedJson, json);
    }

    [TestMethod]
    [DataRow("\"W\"", MatchResult.Win)]
    [DataRow("\"L\"", MatchResult.Loss)]
    [DataRow("\"D\"", MatchResult.Draw)]
    public void Deserialize_ShouldParseSingleLetterCode(string json, MatchResult expected)
    {
        var result = JsonSerializer.Deserialize<MatchResult>(json);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void PlayerResults_ShouldSerializeAsListOfCodes()
    {
        var player = new RankingPlayer { CommunityId = 76561197960287930 };
        player.Results.Add(MatchResult.Win);
        player.Results.Add(MatchResult.Win);
        player.Results.Add(MatchResult.Loss);
        player.Results.Add(MatchResult.Draw);

        var json = JsonSerializer.Serialize(player);

        StringAssert.Contains(json, "\"Results\":[\"W\",\"W\",\"L\",\"D\"]");
    }

    [TestMethod]
    public void PlayerResults_ShouldRoundTrip()
    {
        var player = new RankingPlayer { CommunityId = 76561197960287930 };
        player.Results.Add(MatchResult.Loss);
        player.Results.Add(MatchResult.Draw);
        player.Results.Add(MatchResult.Win);

        var json = JsonSerializer.Serialize(player);
        var restored = JsonSerializer.Deserialize<RankingPlayer>(json)!;

        CollectionAssert.AreEqual(player.Results, restored.Results);
    }
}