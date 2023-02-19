namespace L4D2PlayStats.Core.Modules.Server.Services;

public interface IServerService
{
    Server EnsureAuthentication(string token);
    Server? GetServer(string serverId);
    IEnumerable<Server> GetServers();
}