using L4D2PlayStats.FunctionApp.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.AddApp();
    })
    .ConfigureAppConfiguration((_, config) => { config.AddJsonFile("host.json", true); })
    .Build();

host.Run();