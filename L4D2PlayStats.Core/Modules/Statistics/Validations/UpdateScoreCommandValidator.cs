using FluentValidation;
using L4D2PlayStats.Core.Modules.Statistics.Commands;

namespace L4D2PlayStats.Core.Modules.Statistics.Validations;

public class UpdateScoreCommandValidator : AbstractValidator<UpdateScoreCommand>
{
    public UpdateScoreCommandValidator()
    {
        RuleFor(command => command.TeamAScore)
            .InclusiveBetween(0, 10_000)
            .WithMessage("Team A score must be between 0 and 10,000.");

        RuleFor(command => command.TeamBScore)
            .InclusiveBetween(0, 10_000)
            .WithMessage("Team B score must be between 0 and 10,000.");
    }
}