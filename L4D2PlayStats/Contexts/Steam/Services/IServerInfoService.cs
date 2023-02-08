using L4D2PlayStats.Contexts.Steam.Responses;
using L4D2PlayStats.Contexts.Steam.ValueObjects;
using Refit;

namespace L4D2PlayStats.Contexts.Steam.Services;

public interface IServerInfoService
{
    [Get("/IGameServersService/GetServerList/v1")]
    Task<ResponseData<ServersInfo>> GetServerInfo([AliasAs("key")] string key, [AliasAs("filter")] string filter);
}