using EvilBaschdi.About.Core;
using EvilBaschdi.Core;
using EvilBaschdi.Core.Avalonia;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Dice.Avalonia.DependencyInjection;

/// <inheritdoc />
public class ConfigureAvaloniaServices : IConfigureAvaloniaServices
{
    /// <inheritdoc />
    public void RunFor([NotNull] IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<ICurrentAssembly, CurrentAssembly>();
        services.AddSingleton<IAboutContent, AboutContent>();
    }
}