using L4D2PlayStats.Core.Modules.Punishments.Commands;

namespace L4D2PlayStats.Core.Modules.Punishments.Services;

public interface IPunishmentsService
{
    Task<Punishment> AddOrUpdateAsync(string serverId, PunishmentCommand command);
    Task DeleteAsync(string serverId, string communityId);
}