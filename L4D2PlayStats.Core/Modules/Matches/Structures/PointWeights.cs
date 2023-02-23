namespace L4D2PlayStats.Core.Modules.Matches.Structures;

public class PointWeights
{
    private readonly List<PointWeight> _weights = default!;

    public PointWeights(params PointWeight[] weights)
    {
        Weights = weights.Where(w => !w.Empty).ToList();
    }

    private List<PointWeight> Weights
    {
        get => _weights;
        init
        {
            _weights = value;
            SumOfWeights = value.Select(w => w.Weight).DefaultIfEmpty(0).Sum();
        }
    }

    private decimal SumOfWeights { get; init; }

    public decimal CalculatePoints(decimal points)
    {
        if (SumOfWeights == 0)
            return 0;

        return Weights
            .Select(weight => weight.Percentage * weight.Weight / SumOfWeights)
            .Select(percentage => percentage * points)
            .DefaultIfEmpty(0)
            .Sum();
    }
}