using FluentValidation;
using L4D2PlayStats.Core.Modules.Punishments.Commands;
using L4D2PlayStats.Core.Modules.Punishments.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Core.Modules.Punishments.Services;

public class PunishmentsService(
    IMemoryCache memoryCache,
    IPunishmentsRepository punishmentsRepository,
    IValidator<PunishmentCommand> validator)
    : IPunishmentsService
{
    public async Task<Punishment> AddOrUpdateAsync(string serverId, PunishmentCommand command)
    {
        await validator.ValidateAndThrowAsync(command);

        var punishment = await punishmentsRepository.FindAsync(serverId, command.CommunityId!) ?? new Punishment
        {
            Server = serverId,
            CommunityId = command.CommunityId!
        };

        punishment.LostExperiencePoints += command.LostExperiencePoints;

        await punishmentsRepository.AddOrUpdateAsync(punishment);

        ClearMemoryCache(serverId);

        return punishment;
    }

    public async Task DeleteAsync(string serverId, string communityId)
    {
        await punishmentsRepository.DeleteAsync(serverId, communityId);

        ClearMemoryCache(serverId);
    }

    private void ClearMemoryCache(string serverId)
    {
        memoryCache.Remove($"statistics_{serverId}".ToLower());
        memoryCache.Remove($"matches_{serverId}".ToLower());
        memoryCache.Remove($"ranking_{serverId}".ToLower());
        memoryCache.Remove($"ranking_last_match_{serverId}".ToLower());
        memoryCache.Remove($"player_statistics_{serverId}".ToLower());
    }
}