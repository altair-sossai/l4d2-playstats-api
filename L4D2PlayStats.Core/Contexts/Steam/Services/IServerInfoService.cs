using L4D2PlayStats.Core.Contexts.Steam.Responses;
using L4D2PlayStats.Core.Contexts.Steam.ValueObjects;
using Refit;

namespace L4D2PlayStats.Core.Contexts.Steam.Services;

public interface IServerInfoService
{
    [Get("/IGameServersService/GetServerList/v1")]
    Task<ResponseData<ServersInfo>> GetServerInfoAsync([AliasAs("key")] string key, [AliasAs("filter")] string filter);
}