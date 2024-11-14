namespace L4D2PlayStats.Core.Modules.Punishments.Repositories;

public interface IPunishmentsRepository
{
    ValueTask<Punishment?> FindAsync(string serverId, string communityId);
    IAsyncEnumerable<Punishment> GetPunishmentsAsync(string serverId);
    Task AddOrUpdateAsync(Punishment punishment);
    Task DeleteAsync(string serverId, string communityId);
    Task DeleteAllAsync(string serverId);
}