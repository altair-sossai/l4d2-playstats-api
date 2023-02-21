using FluentValidation;

namespace L4D2PlayStats.Core.Modules.Statistics.Validations.L4D2PlayStats;

public class ScoringValidator : AbstractValidator<Scoring>
{
    public ScoringValidator()
    {
        RuleFor(r => r.TeamA)
            .NotNull();

        RuleFor(r => r.TeamB)
            .NotNull();
    }
}