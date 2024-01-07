using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Dice.Avalonia.DependencyInjection;
using Dice.Avalonia.ViewModels;
using EvilBaschdi.About.Avalonia.DependencyInjection;
using EvilBaschdi.About.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Dice.Avalonia;

/// <inheritdoc />
public class App : Application
{
    /// <inheritdoc />
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    /// <summary>
    ///     ServiceProvider for DependencyInjection
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public static IServiceProvider ServiceProvider { get; set; }

    /// <inheritdoc />
    public override void OnFrameworkInitializationCompleted()
    {
        IServiceCollection serviceCollection = new ServiceCollection();

        IConfigureCoreServices configureCoreServices = new ConfigureCoreServices();
        IConfigureAboutServices configureAboutServices = new ConfigureAboutServices();
        IConfigureAvaloniaServices configureAvaloniaServices = new ConfigureAvaloniaServices();
        IConfigureWindowsAndViewModels configureWindowsAndViewModels = new ConfigureWindowsAndViewModels();

        configureCoreServices.RunFor(serviceCollection);
        configureAboutServices.RunFor(serviceCollection);
        configureAvaloniaServices.RunFor(serviceCollection);
        configureWindowsAndViewModels.RunFor(serviceCollection);

        ServiceProvider = serviceCollection.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainWindow = new MainWindow
                             {
                                 DataContext = ServiceProvider.GetRequiredService<MainWindowViewModel>()
                             };

            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}