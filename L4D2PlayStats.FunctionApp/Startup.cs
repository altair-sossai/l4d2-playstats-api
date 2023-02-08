using L4D2PlayStats.DependencyInjection;
using L4D2PlayStats.FunctionApp;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace L4D2PlayStats.FunctionApp;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddApp();
    }
}