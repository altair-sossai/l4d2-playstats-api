using L4D2PlayStats.Core.Modules.Server.Results;
using Mapster;

namespace L4D2PlayStats.Core.Modules.Server.Registers;

public class ServerRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Server, ServerResult>();
    }
}