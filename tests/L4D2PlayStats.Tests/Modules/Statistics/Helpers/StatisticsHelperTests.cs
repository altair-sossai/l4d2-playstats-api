using FluentAssertions;
using L4D2PlayStats.Core.Modules.Statistics.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace L4D2PlayStats.Tests.Modules.Statistics.Helpers;

[TestClass]
public class StatisticsHelperTests
{
    [TestMethod]
    public void FileNameToRowKey_WithValidFileName_ShouldReturnExpectedResult()
    {
        const string fileName = "2023-02-04_03-13_0011_c8m1_apartment.txt";

        var result = StatisticsHelper.FileNameToRowKey(fileName);

        result.Should().Be("8585261265054665807");
    }

    [TestMethod]
    public void FileNameToRowKey_WithInvalidFileName_ShouldReturnNull()
    {
        const string fileName = "invalid_file_name";

        var result = StatisticsHelper.FileNameToRowKey(fileName);

        result.Should().BeNull();
    }

    [TestMethod]
    public void FileNameToRowKey_WithNullFileName_ShouldReturnNull()
    {
        string? fileName = null;

        var result = StatisticsHelper.FileNameToRowKey(fileName);

        result.Should().BeNull();
    }

    [TestMethod]
    public void CurrentRankingPeriod_WithJanuary_ShouldReturnJanuary()
    {
        var reference = new DateTime(2023, 1, 1);

        var result = StatisticsHelper.CurrentRankingPeriod(reference);

        result.Should().Be(new DateTime(2023, 1, 1));
    }

    [TestMethod]
    public void CurrentRankingPeriod_WithFebruary_ShouldReturnJanuary()
    {
        var reference = new DateTime(2023, 2, 25);

        var result = StatisticsHelper.CurrentRankingPeriod(reference);

        result.Should().Be(new DateTime(2023, 1, 1));
    }

    [TestMethod]
    public void CurrentRankingPeriod_WithMarch_ShouldReturnMarch()
    {
        var reference = new DateTime(2023, 3, 1);

        var result = StatisticsHelper.CurrentRankingPeriod(reference);

        result.Should().Be(new DateTime(2023, 3, 1));
    }

    [TestMethod]
    public void CurrentRankingPeriod_WithApril_ShouldReturnMarch()
    {
        var reference = new DateTime(2023, 4, 1);

        var result = StatisticsHelper.CurrentRankingPeriod(reference);

        result.Should().Be(new DateTime(2023, 3, 1));
    }

    [TestMethod]
    public void CurrentRankingPeriod_WithMay_ShouldReturnMay()
    {
        var reference = new DateTime(2023, 5, 1);

        var result = StatisticsHelper.CurrentRankingPeriod(reference);

        result.Should().Be(new DateTime(2023, 5, 1));
    }

    [TestMethod]
    public void CurrentRankingPeriod_WithJune_ShouldReturnMay()
    {
        var reference = new DateTime(2023, 6, 1);

        var result = StatisticsHelper.CurrentRankingPeriod(reference);

        result.Should().Be(new DateTime(2023, 5, 1));
    }

    [TestMethod]
    public void CurrentRankingPeriod_WithJuly_ShouldReturnJuly()
    {
        var reference = new DateTime(2023, 7, 1);

        var result = StatisticsHelper.CurrentRankingPeriod(reference);

        result.Should().Be(new DateTime(2023, 7, 1));
    }

    [TestMethod]
    public void CurrentRankingPeriod_WithAugust_ShouldReturnJuly()
    {
        var reference = new DateTime(2023, 8, 1);

        var result = StatisticsHelper.CurrentRankingPeriod(reference);

        result.Should().Be(new DateTime(2023, 7, 1));
    }

    [TestMethod]
    public void CurrentRankingPeriod_WithSeptember_ShouldReturnSeptember()
    {
        var reference = new DateTime(2023, 9, 1);

        var result = StatisticsHelper.CurrentRankingPeriod(reference);

        result.Should().Be(new DateTime(2023, 9, 1));
    }

    [TestMethod]
    public void CurrentRankingPeriod_WithOctober_ShouldReturnSeptember()
    {
        var reference = new DateTime(2023, 10, 1);

        var result = StatisticsHelper.CurrentRankingPeriod(reference);

        result.Should().Be(new DateTime(2023, 9, 1));
    }

    [TestMethod]
    public void CurrentRankingPeriod_WithNovember_ShouldReturnNovember()
    {
        var reference = new DateTime(2023, 11, 1);

        var result = StatisticsHelper.CurrentRankingPeriod(reference);

        result.Should().Be(new DateTime(2023, 11, 1));
    }

    [TestMethod]
    public void CurrentRankingPeriod_WithDecember_ShouldReturnNovember()
    {
        var reference = new DateTime(2023, 12, 1);

        var result = StatisticsHelper.CurrentRankingPeriod(reference);

        result.Should().Be(new DateTime(2023, 11, 1));
    }
}