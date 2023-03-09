using L4D2PlayStats.Core.Modules.Mix.Commands;
using L4D2PlayStats.Core.Modules.Mix.Results;

namespace L4D2PlayStats.Core.Modules.Mix.Services;

public interface IMixService
{
    Task<MixResult> MixAsync(string serverId, MixCommand command);
}