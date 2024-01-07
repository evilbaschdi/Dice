using Dice.Avalonia.ViewModels;
using EvilBaschdi.About.Avalonia;
using EvilBaschdi.About.Avalonia.Models;
using EvilBaschdi.Core.Avalonia;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Dice.Avalonia.DependencyInjection;

/// <inheritdoc />
public class ConfigureWindowsAndViewModels : IConfigureWindowsAndViewModels
{
    /// <inheritdoc />
    public void RunFor([NotNull] IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IAboutViewModelExtended, AboutViewModelExtended>();
        services.AddTransient(typeof(AboutWindow));

        services.AddSingleton<IHandleOsDependentTitleBar, HandleOsDependentTitleBar>();
        services.AddSingleton<IApplicationLayout, ApplicationLayout>();
        services.AddSingleton<ITopLevel, MainWindowTopLevel>();
        services.AddSingleton<MainWindowViewModel>();
    }
}