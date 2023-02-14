using System;
using L4D2PlayStats.FunctionApp.Shared.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace L4D2PlayStats.FunctionApp.Functions;

public class PingFunction
{
	private static readonly Guid InstanceId = Guid.NewGuid();

	[FunctionName(nameof(PingFunction) + "_" + nameof(Get))]
	public IActionResult Get([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ping")] HttpRequest httpRequest)
	{
		var result = new
		{
			InstanceId,
			DateTime.Now
		};

		return new JsonResult(result, JsonSettings.DefaultSettings);
	}
}