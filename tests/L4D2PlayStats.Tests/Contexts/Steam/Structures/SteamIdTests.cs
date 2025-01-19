using L4D2PlayStats.Core.Infrastructure.Structures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace L4D2PlayStats.Tests.Contexts.Steam.Structures;

[TestClass]
public class SteamIdTests
{
    [TestMethod]
    public void TryParse_UsingSteamId_Altair()
    {
        var result = SteamIdentifiers.TryParse("STEAM_0:0:90628109", out var userSteamId);
        Assert.IsTrue(result);

        Assert.IsTrue(userSteamId.HasValue);
        Assert.AreEqual(76561198141521946, userSteamId.CommunityId);
    }

    [TestMethod]
    public void TryParse_UsingSteam3_Altair()
    {
        var result = SteamIdentifiers.TryParse("[U:1:181256218]", out var userSteamId);
        Assert.IsTrue(result);

        Assert.IsTrue(userSteamId.HasValue);
        Assert.AreEqual(76561198141521946, userSteamId.CommunityId);
    }

    [TestMethod]
    public void TryParse_UsingCommunityId_Altair()
    {
        var result = SteamIdentifiers.TryParse("76561198141521946", out var userSteamId);
        Assert.IsTrue(result);

        Assert.IsTrue(userSteamId.HasValue);
        Assert.AreEqual(76561198141521946, userSteamId.CommunityId);
    }

    [TestMethod]
    public void TryParse_UsingSteamId_Bruna()
    {
        var result = SteamIdentifiers.TryParse("STEAM_0:1:91233069", out var userSteamId);
        Assert.IsTrue(result);

        Assert.IsTrue(userSteamId.HasValue);
        Assert.AreEqual(76561198142731867, userSteamId.CommunityId);
    }

    [TestMethod]
    public void TryParse_UsingSteam3_Bruna()
    {
        var result = SteamIdentifiers.TryParse("[U:1:182466139]", out var userSteamId);
        Assert.IsTrue(result);

        Assert.IsTrue(userSteamId.HasValue);
        Assert.AreEqual(76561198142731867, userSteamId.CommunityId);
    }

    [TestMethod]
    public void TryParse_UsingCommunityId_Bruna()
    {
        var result = SteamIdentifiers.TryParse("76561198142731867", out var userSteamId);
        Assert.IsTrue(result);

        Assert.IsTrue(userSteamId.HasValue);
        Assert.AreEqual(76561198142731867, userSteamId.CommunityId);
    }

    [TestMethod]
    public void TryParse_UsingSteamId_OhGodANoob()
    {
        var result = SteamIdentifiers.TryParse("STEAM_0:1:11181514", out var userSteamId);
        Assert.IsTrue(result);

        Assert.IsTrue(userSteamId.HasValue);
        Assert.AreEqual(76561197982628757, userSteamId.CommunityId);
    }

    [TestMethod]
    public void TryParse_UsingSteam3_OhGodANoob()
    {
        var result = SteamIdentifiers.TryParse("[U:1:22363029]", out var userSteamId);
        Assert.IsTrue(result);

        Assert.IsTrue(userSteamId.HasValue);
        Assert.AreEqual(76561197982628757, userSteamId.CommunityId);
    }

    [TestMethod]
    public void TryParse_UsingCommunityId_OhGodANoob()
    {
        var result = SteamIdentifiers.TryParse("76561197982628757", out var userSteamId);
        Assert.IsTrue(result);

        Assert.IsTrue(userSteamId.HasValue);
        Assert.AreEqual(76561197982628757, userSteamId.CommunityId);
    }
}