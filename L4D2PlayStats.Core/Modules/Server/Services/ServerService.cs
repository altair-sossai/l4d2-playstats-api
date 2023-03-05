using Azure.Data.Tables;
using L4D2PlayStats.Core.Contexts.AzureTableStorage;
using L4D2PlayStats.Core.Modules.Auth.Commands;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Core.Modules.Server.Services;

public class ServerService : IServerService
{
    private readonly IAzureTableStorageContext _context;
    private readonly IMemoryCache _memoryCache;
    private TableClient? _serverTable;

    public ServerService(IAzureTableStorageContext context,
        IMemoryCache memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
    }

    private TableClient ServerTable => _serverTable ??= _context.GetTableClientAsync("Servers").Result;

    private IEnumerable<Server> Servers => _memoryCache.GetOrCreate("servers", factory =>
    {
        factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);

        return ServerTable.Query<Server>().ToList();
    });

    public Server EnsureAuthentication(string token)
    {
        var command = new AuthenticationCommand(token);
        if (!command.Valid)
            throw new UnauthorizedAccessException();

        var server = Servers.FirstOrDefault(f => f.RowKey == command.ServerId && f.Secret == command.ServerSecret);
        if (server == null)
            throw new UnauthorizedAccessException();

        return server;
    }

    public Server? GetServer(string serverId)
    {
        return Servers.FirstOrDefault(server => server.RowKey == serverId);
    }

    public IEnumerable<Server> GetServers()
    {
        return Servers;
    }
}