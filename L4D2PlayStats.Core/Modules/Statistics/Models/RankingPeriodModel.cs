namespace L4D2PlayStats.Core.Modules.Statistics.Models;

public class RankingPeriodModel
{
    private DateTime _start;

    public RankingPeriodModel(DateTime reference)
    {
        Start = reference.Month switch
        {
            1 or 2 => new DateTime(reference.Year, 1, 1),
            3 or 4 => new DateTime(reference.Year, 3, 1),
            5 or 6 => new DateTime(reference.Year, 5, 1),
            7 or 8 => new DateTime(reference.Year, 7, 1),
            9 or 10 => new DateTime(reference.Year, 9, 1),
            _ => new DateTime(reference.Year, 11, 1)
        };
    }

    public DateTime Start
    {
        get => _start;
        private set
        {
            _start = value;
            End = value.AddMonths(2).AddTicks(-1);
        }
    }

    public DateTime End { get; private set; }

    public RankingPeriodModel PreviousPeriod()
    {
        var reference = Start.AddDays(-1);

        return new RankingPeriodModel(reference);
    }
}