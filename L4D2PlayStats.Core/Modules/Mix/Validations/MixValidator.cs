using FluentValidation;
using L4D2PlayStats.Core.Modules.Mix.Commands;

namespace L4D2PlayStats.Core.Modules.Mix.Validations;

public class MixValidator : AbstractValidator<MixCommand>
{
    public MixValidator()
    {
        RuleFor(r => r.Player1)
            .NotNull()
            .NotEmpty();

        RuleFor(r => r.Player2)
            .NotNull()
            .NotEmpty();

        RuleFor(r => r.Player3)
            .NotNull()
            .NotEmpty();

        RuleFor(r => r.Player4)
            .NotNull()
            .NotEmpty();

        RuleFor(r => r.Player5)
            .NotNull()
            .NotEmpty();

        RuleFor(r => r.Player6)
            .NotNull()
            .NotEmpty();

        RuleFor(r => r.Player7)
            .NotNull()
            .NotEmpty();

        RuleFor(r => r.Player8)
            .NotNull()
            .NotEmpty();
    }
}