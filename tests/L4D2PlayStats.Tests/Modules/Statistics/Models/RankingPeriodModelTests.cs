using L4D2PlayStats.Core.Modules.Statistics.Models;

namespace L4D2PlayStats.Tests.Modules.Statistics.Models;

[TestClass]
public class RankingPeriodModelTests
{
    [TestMethod]
    public void Constructor_JanuaryOrFebruary_ShouldSetCorrectStartAndEndDates()
    {
        var referenceDate = new DateTime(2023, 1, 15);
        var expectedStart = new DateTime(2023, 1, 1);
        var expectedEnd = new DateTime(2023, 2, 28, 23, 59, 59, 999);

        var model = new RankingPeriodModel(referenceDate);

        Assert.AreEqual(expectedStart, model.Start);
        Assert.AreEqual(expectedEnd.ToString("u"), model.End.ToString("u"));
    }

    [TestMethod]
    public void Constructor_MarchOrApril_ShouldSetCorrectStartAndEndDates()
    {
        var referenceDate = new DateTime(2023, 3, 10);
        var expectedStart = new DateTime(2023, 3, 1);
        var expectedEnd = new DateTime(2023, 4, 30, 23, 59, 59, 999);

        var model = new RankingPeriodModel(referenceDate);

        Assert.AreEqual(expectedStart, model.Start);
        Assert.AreEqual(expectedEnd.ToString("u"), model.End.ToString("u"));
    }

    [TestMethod]
    public void Constructor_MayOrJune_ShouldSetCorrectStartAndEndDates()
    {
        var referenceDate = new DateTime(2023, 6, 15);
        var expectedStart = new DateTime(2023, 5, 1);
        var expectedEnd = new DateTime(2023, 6, 30, 23, 59, 59, 999);

        var model = new RankingPeriodModel(referenceDate);

        Assert.AreEqual(expectedStart, model.Start);
        Assert.AreEqual(expectedEnd.ToString("u"), model.End.ToString("u"));
    }

    [TestMethod]
    public void Constructor_JulyOrAugust_ShouldSetCorrectStartAndEndDates()
    {
        var referenceDate = new DateTime(2023, 8, 10);
        var expectedStart = new DateTime(2023, 7, 1);
        var expectedEnd = new DateTime(2023, 8, 31, 23, 59, 59, 999);

        var model = new RankingPeriodModel(referenceDate);

        Assert.AreEqual(expectedStart, model.Start);
        Assert.AreEqual(expectedEnd.ToString("u"), model.End.ToString("u"));
    }

    [TestMethod]
    public void Constructor_SeptemberOrOctober_ShouldSetCorrectStartAndEndDates()
    {
        var referenceDate = new DateTime(2023, 9, 5);
        var expectedStart = new DateTime(2023, 9, 1);
        var expectedEnd = new DateTime(2023, 10, 31, 23, 59, 59, 999);

        var model = new RankingPeriodModel(referenceDate);

        Assert.AreEqual(expectedStart, model.Start);
        Assert.AreEqual(expectedEnd.ToString("u"), model.End.ToString("u"));
    }

    [TestMethod]
    public void Constructor_NovemberOrDecember_ShouldSetCorrectStartAndEndDates()
    {
        var referenceDate = new DateTime(2023, 12, 20);
        var expectedStart = new DateTime(2023, 11, 1);
        var expectedEnd = new DateTime(2023, 12, 31, 23, 59, 59, 999);

        var model = new RankingPeriodModel(referenceDate);

        Assert.AreEqual(expectedStart, model.Start);
        Assert.AreEqual(expectedEnd.ToString("u"), model.End.ToString("u"));
    }
}