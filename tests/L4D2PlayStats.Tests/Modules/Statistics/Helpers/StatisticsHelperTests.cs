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
}