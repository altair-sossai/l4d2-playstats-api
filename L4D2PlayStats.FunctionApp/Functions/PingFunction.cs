using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace L4D2PlayStats.FunctionApp.Functions;

public class PingFunction
{
    private static readonly Guid InstanceId = Guid.NewGuid();

    [Function(nameof(PingFunction) + "_" + nameof(Get))]
    public IActionResult Get([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ping")] HttpRequest httpRequest)
    {
        var result = new
        {
            InstanceId,
            DateTime.Now
        };

        return new JsonResult(result);
    }
}