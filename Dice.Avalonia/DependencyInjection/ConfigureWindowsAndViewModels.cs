﻿using Dice.Avalonia.ViewModels;
using EvilBaschdi.About.Avalonia;
using EvilBaschdi.About.Avalonia.Models;
using EvilBaschdi.Core.Avalonia;
using Microsoft.Extensions.DependencyInjection;

namespace Dice.Avalonia.DependencyInjection;

/// <summary />
public static class ConfigureWindowsAndViewModels
{
    /// <summary />
    public static void AddWindowsAndViewModels(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IAboutViewModelExtended, AboutViewModelExtended>();
        services.AddTransient<AboutWindow>();

        services.AddSingleton<IHandleOsDependentTitleBar, HandleOsDependentTitleBar>();
        services.AddSingleton<IApplicationLayout, ApplicationLayout>();
        services.AddSingleton<ITopLevel, MainWindowTopLevel>();
        services.AddSingleton<MainWindowViewModel>();
    }
}