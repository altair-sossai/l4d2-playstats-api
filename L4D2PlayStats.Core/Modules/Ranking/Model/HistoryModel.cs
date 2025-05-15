using System.Text.RegularExpressions;

namespace L4D2PlayStats.Core.Modules.Ranking.Model;

public class HistoryModel
{
    private const string Pattern = @"^ranking_(\d{4})(\d{2})(\d{4})(\d{2})\.json$";
    private static readonly Regex Regex = new(Pattern);
    private string _fileName = null!;

    public HistoryModel(string fileName)
    {
        FileName = fileName;
    }

    public string Id => $"{StartYear:0000}{StartMonth:00}{EndYear:0000}{EndMonth:00}";

    public string FileName
    {
        get => _fileName;
        set
        {
            _fileName = value;

            var match = Regex.Match(value);

            StartYear = int.Parse(match.Groups[1].Value);
            StartMonth = int.Parse(match.Groups[2].Value);

            EndYear = int.Parse(match.Groups[3].Value);
            EndMonth = int.Parse(match.Groups[4].Value);
        }
    }

    public int StartYear { get; private set; }
    public int StartMonth { get; private set; }
    public int EndYear { get; private set; }
    public int EndMonth { get; private set; }

    public static HistoryModel? Parse(string fileName)
    {
        return Regex.IsMatch(fileName) ? new HistoryModel(fileName) : null;
    }
}