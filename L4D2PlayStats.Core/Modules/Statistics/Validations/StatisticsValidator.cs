using FluentValidation;

namespace L4D2PlayStats.Core.Modules.Statistics.Validations;

public class StatisticsValidator : AbstractValidator<Statistics>
{
    public StatisticsValidator(IValidator<global::L4D2PlayStats.Statistics> statisticsValidator)
    {
        RuleFor(r => r.Server)
            .NotNull()
            .NotEmpty();

        RuleFor(r => r.FileName)
            .NotNull()
            .NotEmpty();

        RuleFor(r => r.Round)
            .GreaterThan(0);

        RuleFor(r => r.TeamSize)
            .GreaterThan(0);

        RuleFor(r => r.ConfigurationName)
            .NotNull()
            .NotEmpty();

        RuleFor(r => r.MapName)
            .NotNull()
            .NotEmpty();

        RuleFor(r => r.Content)
            .NotNull()
            .NotEmpty();

        RuleFor(r => r.Statistic)
            .NotNull()
            .SetValidator(statisticsValidator!);

        RuleFor(r => r.PartitionKey)
            .NotNull()
            .NotEmpty();

        RuleFor(r => r.RowKey)
            .NotNull()
            .NotEmpty();
    }
}