using AutoMapper;
using L4D2PlayStats.Core.Modules.Server.Results;

namespace L4D2PlayStats.Core.Modules.Server.Profiles;

public class ServerProfile : Profile
{
    public ServerProfile()
    {
        CreateMap<Server, ServerResult>();
    }
}