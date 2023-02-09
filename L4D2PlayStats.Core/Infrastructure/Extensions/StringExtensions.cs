using System.Text.RegularExpressions;

namespace L4D2PlayStats.Core.Infrastructure.Extensions;

public static class StringExtensions
{
    public static string FirstLetterToLower(this string s)
    {
        if (string.IsNullOrEmpty(s))
            return string.Empty;

        return s.Length == 1 ? s.ToLower() : $"{char.ToLower(s[0])}{s[1..]}";
    }

    public static string? MatchValue(this string input, IEnumerable<string> patterns)
    {
        if (string.IsNullOrEmpty(input))
            return null;

        var pattern = patterns.FirstOrDefault(pattern => Regex.IsMatch(input, pattern));

        if (string.IsNullOrEmpty(pattern))
            return null;

        var match = Regex.Match(input, pattern);
        var group = match.Groups[1];

        return group.Value;
    }
}