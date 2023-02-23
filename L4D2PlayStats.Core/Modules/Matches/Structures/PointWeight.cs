namespace L4D2PlayStats.Core.Modules.Matches.Structures;

public class PointWeight
{
    private readonly decimal _total;
    private readonly decimal _value;
    private readonly IReadOnlyCollection<decimal> _values = default!;

    public PointWeight(decimal value, IEnumerable<int> values, decimal weight = 1)
        : this(value, values.Select(i => (decimal)i), weight)
    {
    }

    private PointWeight(decimal value, IEnumerable<decimal> values, decimal weight = 1)
    {
        Value = value;
        Values = values.ToList();
        Weight = weight;
    }

    private decimal Value
    {
        get => _value;
        init
        {
            _value = value;
            UpdatePercentage();
        }
    }

    private IReadOnlyCollection<decimal> Values
    {
        get => _values;
        init
        {
            _values = value;
            Total = value.DefaultIfEmpty(0).Sum();
        }
    }

    public bool Empty => Values.Count == 0 || Values.All(v => v == 0);
    public decimal Weight { get; }

    private decimal Total
    {
        get => _total;
        init
        {
            _total = value;
            UpdatePercentage();
        }
    }

    public decimal Percentage { get; private set; }

    private void UpdatePercentage()
    {
        Percentage = Total == 0 ? 0 : Value / Total;
    }
}