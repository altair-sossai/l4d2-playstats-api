using System.Reflection;
using FluentValidation;
using L4D2PlayStats.Contexts.Steam.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace L4D2PlayStats.DependencyInjection;

public static class AppInjection
{
    public static void AddApp(this IServiceCollection serviceCollection)
    {
        var assemblies = new[]
        {
            Assembly.Load("L4D2PlayStats")
        };

        serviceCollection.AddAutoMapper(assemblies);
        serviceCollection.AddValidatorsFromAssemblies(assemblies);
        serviceCollection.AddMemoryCache();

        serviceCollection.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses()
            .AsImplementedInterfaces(type => assemblies.Contains(type.Assembly)));

        serviceCollection.AddSteamContext();
    }
}