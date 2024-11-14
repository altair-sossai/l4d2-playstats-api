using FluentValidation;
using L4D2PlayStats.Core.Modules.Punishments.Commands;

namespace L4D2PlayStats.Core.Modules.Punishments.Validations;

public class PunishmentCommandValidator : AbstractValidator<PunishmentCommand>
{
    public PunishmentCommandValidator()
    {
        RuleFor(r => r.CommunityId)
            .NotNull()
            .NotEmpty();

        RuleFor(r => r.LostExperiencePoints)
            .GreaterThan(0);
    }
}