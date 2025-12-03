using System.Text.RegularExpressions;

namespace L4D2PlayStats.Core.Infrastructure.Extensions;

public static class StringExtensions
{
    extension(string input)
    {
        public string FirstLetterToLower()
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return input.Length == 1 ? input.ToLower() : $"{char.ToLower(input[0])}{input[1..]}";
        }

        public string? MatchValue(IEnumerable<string> patterns)
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
}