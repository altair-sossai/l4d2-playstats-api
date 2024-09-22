using System.Text.RegularExpressions;

namespace L4D2PlayStats.Core.Modules.Ranking.Model;

public class HistoryModel
{
    private const string Pattern = @"^ranking_(\d{4})\-(\d{2})_(\d{4})\-(\d{2})\.json$";
    private static readonly Regex Regex = new(Pattern);
    private string _fileName = default!;

    public HistoryModel(string fileName)
    {
        FileName = fileName;
    }

    public string Id { get; private set; } = default!;

    public string FileName
    {
        get => _fileName;
        set
        {
            _fileName = value;

            Id = value[..^5];

            var match = Regex.Match(value);

            StartMonth = int.Parse(match.Groups[1].Value);
            StartYear = int.Parse(match.Groups[2].Value);

            EndMonth = int.Parse(match.Groups[3].Value);
            EndYear = int.Parse(match.Groups[4].Value);
        }
    }

    public int StartMonth { get; private set; }
    public int StartYear { get; private set; }
    public int EndMonth { get; private set; }
    public int EndYear { get; private set; }

    public static HistoryModel? Parse(string fileName)
    {
        return Regex.IsMatch(fileName) ? new HistoryModel(fileName) : null;
    }
}