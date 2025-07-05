using System.Linq;
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace L4D2PlayStats.FunctionApp.DependencyInjection;

public static class AppInjection
{
    public static void AddApp(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var assemblies = new[]
        {
            Assembly.Load("L4D2PlayStats.Core")
        };

        serviceCollection.AddAutoMapper(c =>
        {
            c.LicenseKey = configuration.GetValue<string>("AutoMapperLicenseKey")!;
            c.AddMaps(assemblies);
        });

        serviceCollection.AddValidatorsFromAssemblies(assemblies);
        serviceCollection.AddMemoryCache();

        serviceCollection.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses()
            .AsImplementedInterfaces(type => assemblies.Contains(type.Assembly)));
    }
}