using System.Text.RegularExpressions;

namespace L4D2PlayStats.Core.Modules.Ranking.Model;

public class HistoryModel
{
    private const string BimonthlyPattern = @"^ranking_(\d{4})(\d{2})(\d{4})(\d{2})\.json$";
    private const string AnnualPattern = @"^ranking_(\d{4})\.json$";

    private static readonly Regex BimonthlyRegex = new(BimonthlyPattern);
    private static readonly Regex AnnualRegex = new(AnnualPattern);

    private string _fileName = null!;

    public HistoryModel(string fileName)
    {
        FileName = fileName;
    }

    public string Id => IsAnnual ? $"{StartYear:0000}" : $"{StartYear:0000}{StartMonth:00}{EndYear:0000}{EndMonth:00}";

    public string FileName
    {
        get => _fileName;
        set
        {
            _fileName = value;

            if (IsBimonthly)
            {
                var match = BimonthlyRegex.Match(value);

                StartYear = int.Parse(match.Groups[1].Value);
                StartMonth = int.Parse(match.Groups[2].Value);

                EndYear = int.Parse(match.Groups[3].Value);
                EndMonth = int.Parse(match.Groups[4].Value);
            }

            if (IsAnnual)
            {
                var match = AnnualRegex.Match(value);
                StartYear = int.Parse(match.Groups[1].Value);
                StartMonth = 1;
                EndYear = StartYear;
                EndMonth = 12;
            }
        }
    }

    public int StartYear { get; private set; }
    public int StartMonth { get; private set; }
    public int EndYear { get; private set; }
    public int EndMonth { get; private set; }

    public bool IsBimonthly => BimonthlyRegex.IsMatch(FileName);
    public bool IsAnnual => AnnualRegex.IsMatch(FileName);

    public static HistoryModel? Parse(string fileName)
    {
        return BimonthlyRegex.IsMatch(fileName) || AnnualRegex.IsMatch(fileName) ? new HistoryModel(fileName) : null;
    }
}