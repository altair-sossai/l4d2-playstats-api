using FluentValidation;

namespace L4D2PlayStats.Core.Modules.Statistics.Validations.L4D2PlayStats;

public class HalfValidator : AbstractValidator<global::L4D2PlayStats.Statistics.Half>
{
    public HalfValidator()
    {
        RuleFor(r => r.RoundHalf)
            .NotNull();

        RuleFor(r => r.Progress)
            .NotNull();

        RuleFor(r => r.Players)
            .NotNull()
            .Must(players => players.Any());

        RuleFor(r => r.InfectedPlayers)
            .NotNull()
            .Must(infectedPlayers => infectedPlayers.Any());
    }
}