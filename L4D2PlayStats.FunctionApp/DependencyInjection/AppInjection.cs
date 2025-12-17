using System.Linq;
using System.Reflection;
using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace L4D2PlayStats.FunctionApp.DependencyInjection;

public static class AppInjection
{
    public static void AddApp(this IServiceCollection serviceCollection)
    {
        var assemblies = new[]
        {
            Assembly.Load("L4D2PlayStats.Core")
        };

        serviceCollection.AddMapster();

        var config = TypeAdapterConfig.GlobalSettings;

        config.Scan(assemblies);
        config.Compile();

        serviceCollection.AddValidatorsFromAssemblies(assemblies);
        serviceCollection.AddMemoryCache();

        serviceCollection.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses()
            .AsImplementedInterfaces(type => assemblies.Contains(type.Assembly)));
    }
}