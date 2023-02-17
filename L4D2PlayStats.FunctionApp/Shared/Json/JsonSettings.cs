using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace L4D2PlayStats.FunctionApp.Shared.Json;

public static class JsonSettings
{
    public static readonly JsonSerializerSettings DefaultSettings = new()
    {
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        },
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        DateTimeZoneHandling = DateTimeZoneHandling.Utc
    };
}