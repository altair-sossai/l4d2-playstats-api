using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace L4D2PlayStats.FunctionApp.Extensions;

public static class HttpRequestDataExtensions
{
    private static readonly JsonSerializerOptions Settings = new()
    {
        PropertyNameCaseInsensitive = true
    };

    extension(HttpRequest httpRequest)
    {
        public async Task<T> DeserializeBodyAsync<T>()
        {
            using var streamReader = new StreamReader(httpRequest.Body);
            var json = await streamReader.ReadToEndAsync();
            var t = JsonSerializer.Deserialize<T>(json, Settings);

            return t;
        }

        public string AuthorizationToken()
        {
            return httpRequest.Headers["Authorization"].ToString();
        }
    }
}