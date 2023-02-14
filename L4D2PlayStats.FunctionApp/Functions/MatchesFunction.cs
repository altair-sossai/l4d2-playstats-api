using System;
using System.Threading.Tasks;
using L4D2PlayStats.Core.Modules.Matches.Services;
using L4D2PlayStats.FunctionApp.Errors;
using L4D2PlayStats.FunctionApp.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.FunctionApp.Functions;

public class MatchesFunction
{
	private const int StatisticsCount = 250;

	private readonly IMatchService _matchService;
	private readonly IMemoryCache _memoryCache;

	public MatchesFunction(IMemoryCache memoryCache, IMatchService matchService)
	{
		_memoryCache = memoryCache;
		_matchService = matchService;
	}

	[FunctionName(nameof(MatchesFunction) + "_" + nameof(GetMatchesAsync))]
	public async Task<IActionResult> GetMatchesAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "matches/{server}")] HttpRequest httpRequest,
		string server)
	{
		try
		{
			var results = await _memoryCache.GetOrCreateAsync($"matches_{server}".ToLower(), async factory =>
			{
				factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

				var matches = await _matchService.GetMatchesAsync(server, StatisticsCount);

				return matches;
			});

			return new OkObjectResult(results);
		}
		catch (Exception exception)
		{
			return ErrorResult.Build(exception).ResponseMessageResult();
		}
	}
}