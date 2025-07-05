using L4D2PlayStats.FunctionApp.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((host, services) => { services.AddApp(host.Configuration); })
    .ConfigureAppConfiguration((_, config) => { config.AddJsonFile("host.json", true); })
    .Build();

host.Run();