using Azure.Data.Tables;
using L4D2PlayStats.Core.Contexts.AzureTableStorage;
using L4D2PlayStats.Core.Modules.Auth.Commands;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Core.Modules.Server.Services;

public class ServerService(
    IAzureTableStorageContext context,
    IMemoryCache memoryCache)
    : IServerService
{
    private TableClient? _serverTable;

    private TableClient ServerTable => _serverTable ??= context.GetTableClientAsync("Servers").Result;

    private IEnumerable<Server> Servers => memoryCache.GetOrCreate("servers", factory =>
    {
        factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);

        return ServerTable.Query<Server>().ToList();
    })!;

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