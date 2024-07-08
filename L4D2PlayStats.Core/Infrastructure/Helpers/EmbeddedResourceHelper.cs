using System.Reflection;

namespace L4D2PlayStats.Core.Infrastructure.Helpers;

public static class EmbeddedResourceHelper
{
    private static readonly Assembly Assembly;

    static EmbeddedResourceHelper()
    {
        Assembly = Assembly.GetExecutingAssembly();
    }

    public static async Task<string> LoadEmbeddedResourceAsync(string resourceName)
    {
        await using var stream = Assembly.GetManifestResourceStream(resourceName);

        if (stream == null)
            throw new ArgumentException($"Resource not found: {resourceName}");

        using var reader = new StreamReader(stream);

        return await reader.ReadToEndAsync();
    }
}