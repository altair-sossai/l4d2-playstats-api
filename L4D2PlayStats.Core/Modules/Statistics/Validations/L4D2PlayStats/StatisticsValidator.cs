using FluentValidation;

namespace L4D2PlayStats.Core.Modules.Statistics.Validations.L4D2PlayStats;

public class StatisticsValidator : AbstractValidator<global::L4D2PlayStats.Statistics>
{
    public StatisticsValidator(IValidator<global::L4D2PlayStats.Statistics.Half> halfValidator,
        IValidator<Scoring> scoringValidator)
    {
        RuleFor(r => r.GameRound)
            .NotNull();

        RuleFor(r => r.Halves)
            .NotNull()
            .Must(halves => halves.Count == 2);

        RuleForEach(r => r.Halves)
            .SetValidator(halfValidator);

        RuleFor(r => r.Scoring)
            .NotNull()
            .SetValidator(scoringValidator!);

        RuleFor(r => r.PlayerNames)
            .NotNull()
            .Must(playerNames => playerNames.Any());

        RuleFor(r => r.TeamA)
            .NotNull()
            .Must(teamA => teamA.Any());

        RuleFor(r => r.TeamB)
            .NotNull()
            .Must(teamB => teamB.Any());

        RuleFor(r => r.MapStart)
            .NotNull();

        RuleFor(r => r.MapEnd)
            .NotNull();
    }
}