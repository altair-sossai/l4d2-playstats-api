using Microsoft.AspNetCore.Http;

namespace L4D2PlayStats.FunctionApp.Extensions;

public static class QueryCollectionExtensions
{
	public static int Int32Value(this IQueryCollection queryCollection, string key, int defaultValue)
	{
		if (!queryCollection.ContainsKey(key))
			return defaultValue;

		return int.TryParse(queryCollection[key], out var value) ? value : defaultValue;
	}
}