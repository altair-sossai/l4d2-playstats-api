using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace L4D2PlayStats.FunctionApp.Extensions;

public static class HttpRequestDataExtensions
{
    public static async Task<T> DeserializeBodyAsync<T>(this HttpRequest httpRequest)
    {
        using var streamReader = new StreamReader(httpRequest.Body);
        var json = await streamReader.ReadToEndAsync();
        var t = JsonSerializer.Deserialize<T>(json);

        return t;
    }

    public static string AuthorizationToken(this HttpRequest httpRequest)
    {
        return httpRequest.Headers["Authorization"].ToString();
    }
}