using FluentAssertions;
using L4D2PlayStats.Core.Contexts.Steam.Structures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace L4D2PlayStats.Tests.Contexts.Steam.Structures;

[TestClass]
public class SteamIdTests
{
    [TestMethod]
    public void TryParse_UsingSteamId_Altair()
    {
        SteamIdentifiers.TryParse("STEAM_0:0:90628109", out var userSteamId).Should().BeTrue();

        userSteamId.HasValue.Should().BeTrue();
        userSteamId.CommunityId.Should().Be(76561198141521946);
    }

    [TestMethod]
    public void TryParse_UsingSteam3_Altair()
    {
        SteamIdentifiers.TryParse("[U:1:181256218]", out var userSteamId).Should().BeTrue();

        userSteamId.HasValue.Should().BeTrue();
        userSteamId.CommunityId.Should().Be(76561198141521946);
    }

    [TestMethod]
    public void TryParse_UsingCommunityId_Altair()
    {
        SteamIdentifiers.TryParse("76561198141521946", out var userSteamId).Should().BeTrue();

        userSteamId.HasValue.Should().BeTrue();
        userSteamId.CommunityId.Should().Be(76561198141521946);
    }

    [TestMethod]
    public void TryParse_UsingSteamId_Bruna()
    {
        SteamIdentifiers.TryParse("STEAM_0:1:91233069", out var userSteamId).Should().BeTrue();

        userSteamId.HasValue.Should().BeTrue();
        userSteamId.CommunityId.Should().Be(76561198142731867);
    }

    [TestMethod]
    public void TryParse_UsingSteam3_Bruna()
    {
        SteamIdentifiers.TryParse("[U:1:182466139]", out var userSteamId).Should().BeTrue();

        userSteamId.HasValue.Should().BeTrue();
        userSteamId.CommunityId.Should().Be(76561198142731867);
    }

    [TestMethod]
    public void TryParse_UsingCommunityId_Bruna()
    {
        SteamIdentifiers.TryParse("76561198142731867", out var userSteamId).Should().BeTrue();

        userSteamId.HasValue.Should().BeTrue();
        userSteamId.CommunityId.Should().Be(76561198142731867);
    }

    [TestMethod]
    public void TryParse_UsingSteamId_OhGodANoob()
    {
        SteamIdentifiers.TryParse("STEAM_0:1:11181514", out var userSteamId).Should().BeTrue();

        userSteamId.HasValue.Should().BeTrue();
        userSteamId.CommunityId.Should().Be(76561197982628757);
    }

    [TestMethod]
    public void TryParse_UsingSteam3_OhGodANoob()
    {
        SteamIdentifiers.TryParse("[U:1:22363029]", out var userSteamId).Should().BeTrue();

        userSteamId.HasValue.Should().BeTrue();
        userSteamId.CommunityId.Should().Be(76561197982628757);
    }

    [TestMethod]
    public void TryParse_UsingCommunityId_OhGodANoob()
    {
        SteamIdentifiers.TryParse("76561197982628757", out var userSteamId).Should().BeTrue();

        userSteamId.HasValue.Should().BeTrue();
        userSteamId.CommunityId.Should().Be(76561197982628757);
    }
}