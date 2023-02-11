using System.Text.RegularExpressions;

namespace L4D2PlayStats.Core.Modules.Statistics.Helpers;

public static class StatisticsHelper
{
    private const string Pattern = @"^(\d{4})-(\d{2})-(\d{2})_(\d{2})-(\d{2})_(\d{4})_.+\.txt$";
    private static readonly Regex Regex = new(Pattern);

    public static string? FileNameToRowKey(string? fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            return null;

        var match = Regex.Match(fileName);
        if (!match.Success)
            return null;

        var year = int.Parse(match.Groups[1].Value);
        var month = int.Parse(match.Groups[2].Value);
        var day = int.Parse(match.Groups[3].Value);
        var hour = int.Parse(match.Groups[4].Value);
        var minute = int.Parse(match.Groups[5].Value);
        var sequence = int.Parse(match.Groups[6].Value);

        var dateTime = new DateTime(year, month, day, hour, minute, 0, sequence, DateTimeKind.Utc);

        return $"{long.MaxValue - dateTime.Ticks}";
    }
}