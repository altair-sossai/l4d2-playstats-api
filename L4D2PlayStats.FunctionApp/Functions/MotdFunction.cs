using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace L4D2PlayStats.FunctionApp.Functions;

public class MotdFunction
{
    [Function(nameof(MotdFunction) + "_" + nameof(RankingAsync))]
    public async Task<IActionResult> RankingAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "motd/{serverId}/ranking/{communityId:long}")] HttpRequest httpRequest,
        string serverId, long communityId)
    {
        return new ContentResult
        {
            Content = "<html><head><title>Zone Server</title></head><body>OI</body></html>",
            ContentType = "text/html",
            StatusCode = 200
        };
    }
}