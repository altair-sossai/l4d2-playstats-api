using System.Text.RegularExpressions;

namespace L4D2PlayStats.Core.Modules.Statistics.Helpers;

public static class StatisticsHelper
{
    private const string Pattern = @"^(\d{4})-(\d{2})-(\d{2})_(\d{2})-(\d{2})_(\d{4})_.+\.txt$";
    private static readonly Regex Regex = new(Pattern);

    public static DateTime? FileNameToDateTime(string? fileName)
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

        return dateTime;
    }

    public static string? FileNameToRowKey(string? fileName)
    {
        var dateTime = FileNameToDateTime(fileName);

        return dateTime == null ? null : $"{long.MaxValue - dateTime.Value.Ticks}";
    }

    public static DateTime CurrentRankingPeriod(DateTime reference)
    {
        return reference.Month switch
        {
            1 or 2 => new DateTime(reference.Year, 1, 1),
            3 or 4 => new DateTime(reference.Year, 3, 1),
            5 or 6 => new DateTime(reference.Year, 5, 1),
            7 or 8 => new DateTime(reference.Year, 7, 1),
            9 or 10 => new DateTime(reference.Year, 9, 1),
            _ => new DateTime(reference.Year, 11, 1)
        };
    }
}