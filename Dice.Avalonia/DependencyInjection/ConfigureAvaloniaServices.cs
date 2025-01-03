using EvilBaschdi.About.Core;
using EvilBaschdi.Core;
using EvilBaschdi.Core.Avalonia;
using Microsoft.Extensions.DependencyInjection;

namespace Dice.Avalonia.DependencyInjection;

/// <summary />
public static class ConfigureAvaloniaServices
{
    /// <summary />
    public static void AddAvaloniaServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<ICurrentAssembly, CurrentAssembly>();
        services.AddSingleton<IAboutContent, AboutContent>();
    }
}