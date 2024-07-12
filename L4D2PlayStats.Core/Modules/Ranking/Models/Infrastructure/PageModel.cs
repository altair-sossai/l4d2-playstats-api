using System.Globalization;
using L4D2PlayStats.Core.Infrastructure.Extensions;
using L4D2PlayStats.Core.Infrastructure.Helpers;
using NUglify;
using Stubble.Core;
using Stubble.Core.Builders;
using Stubble.Helpers;

namespace L4D2PlayStats.Core.Modules.Ranking.Models.Infrastructure;

public abstract class PageModel(string serverId, string resourceName)
{
    private static readonly Dictionary<string, string> Templates = new();
    private static readonly CultureInfo CultureInfo = new("en-us");

    private static StubbleVisitorRenderer? _stuble;

    static PageModel()
    {
        Helpers = new Stubble.Helpers.Helpers()
            .Register("N0", (HelperContext _, int value) => value.ToString("N0", CultureInfo))
            .Register("P0", (HelperContext _, decimal value) => value.ToString("P0", CultureInfo))
            .Register("Truncate12", (HelperContext _, string value) => value.Truncate(12))
            .Register("Truncate30", (HelperContext _, string value) => value.Truncate(30))
            .Register("Time", (HelperContext _, TimeSpan? value) => value?.ToString(@"hh\:mm\:ss"));
    }

    public string ServerId { get; } = serverId;
    public string UrlRanking => $"http://l4d2playstats.blob.core.windows.net/assets/{ServerId}-ranking.html";
    public string UrlLastMatch => $"http://l4d2playstats.blob.core.windows.net/assets/{ServerId}-last-match.html";

    private static StubbleVisitorRenderer Stuble
    {
        get
        {
            if (_stuble != null)
                return _stuble;

            var stubbleBuilder = new StubbleBuilder();

            stubbleBuilder.Configure(settings => { settings.AddHelpers(Helpers); });

            return _stuble = stubbleBuilder.Build();
        }
    }

    private static Stubble.Helpers.Helpers Helpers { get; }

    public async Task<Stream> RenderAsync()
    {
        if (!Templates.ContainsKey(resourceName))
            Templates.Add(resourceName, await EmbeddedResourceHelper.LoadEmbeddedResourceAsync(resourceName));

        var template = Templates[resourceName];

        var html = await Stuble.RenderAsync(template, this);
        var uglifyResult = Uglify.Html(html);

        var memoryStream = new MemoryStream();
        var streamWriter = new StreamWriter(memoryStream);

        await streamWriter.WriteAsync(uglifyResult.Code);
        await streamWriter.FlushAsync();

        memoryStream.Position = 0;

        return memoryStream;
    }
}