using L4D2PlayStats.DependencyInjection;
using L4D2PlayStats.FunctionApp;
using L4D2PlayStats.FunctionApp.Shared.Json;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Newtonsoft.Json;

[assembly: FunctionsStartup(typeof(Startup))]

namespace L4D2PlayStats.FunctionApp;

public class Startup : FunctionsStartup
{
	public override void Configure(IFunctionsHostBuilder builder)
	{
		JsonConvert.DefaultSettings = () => JsonSettings.DefaultSettings;

		builder.Services.AddApp();
	}
}