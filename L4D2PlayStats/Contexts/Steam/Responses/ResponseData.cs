using System.Text.Json.Serialization;

namespace L4D2PlayStats.Contexts.Steam.Responses;

public class ResponseData<T>
    where T : class
{
    [JsonPropertyName("response")]
    public T? Response { get; set; }
}