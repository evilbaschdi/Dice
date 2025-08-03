using System.Reactive;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Dice.Core;
using Dice.Core.Settings;
using EvilBaschdi.About.Avalonia;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Dice.Avalonia.ViewModels;

/// <summary>
///     The main window view model.
/// </summary>
public class MainWindowViewModel : ViewModelBase
{
    private readonly IRollTheDice _rollTheDice;
    private readonly ITopLevel _topLevel;
    private readonly IInitialDirectoryFromSettings _initialDirectoryFromSettings;

    /// <summary>
    ///     Gets or Sets the about window command.
    /// </summary>
    public ReactiveCommand<Unit, Unit> AboutWindowCommand { get; set; }

    /// <summary>
    ///     Gets or Sets the about window command.
    /// </summary>
    public ReactiveCommand<Unit, Unit> BrowseInitialDirectoryCommand { get; set; }

    /// <summary>
    ///     Gets or Sets the about window command.
    /// </summary>
    public ReactiveCommand<Unit, Unit> ThrowTheDiceCommand { get; set; }

    /// <summary>
    ///     Binding
    /// </summary>
    public string ThrowTheDiceContentText
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    } = "roll the dice";

    private async Task AboutWindowCommandAction()
    {
        var aboutWindow = App.ServiceProvider.GetRequiredService<AboutWindow>();
        var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;
        if (mainWindow != null)
        {
            await aboutWindow.ShowDialog(mainWindow);
        }
    }

    private async Task BrowseInitialDirectoryCommandAction()
    {
        var storageProvider = _topLevel.Value.StorageProvider;

        var folderPickerOpenOptions = new FolderPickerOpenOptions
                                      {
                                          Title = "Choose Directory to dice",
                                          //SuggestedStartLocation = 
                                          AllowMultiple = false
                                      };
        var folderPicker = await storageProvider.OpenFolderPickerAsync(folderPickerOpenOptions);

#pragma warning disable CA1826
        var storageFolder = folderPicker.FirstOrDefault();
#pragma warning restore CA1826
        var fullPath = FullPathOrName(storageFolder);
        //InitialDirectory.Text = fullPath;
        _initialDirectoryFromSettings.Value = fullPath;
    }

    private static string FullPathOrName(IStorageItem item) => item is null ? "(null)" : item.Path.LocalPath;

    //return item.TryGetUri(out var uri) ? uri.LocalPath : item.Name;
    private async Task ThrowTheDiceCommandAction()
    {
        ThrowTheDiceContentText = await _rollTheDice.ValueAsync();
    }

    /// <summary>
    ///     Constructor
    /// </summary>
    public MainWindowViewModel([NotNull] IRollTheDice rollTheDice,
                               [NotNull] ITopLevel topLevel,
                               [NotNull] IInitialDirectoryFromSettings initialDirectoryFromSettings
    )
    {
        _rollTheDice = rollTheDice ?? throw new ArgumentNullException(nameof(rollTheDice));
        _topLevel = topLevel ?? throw new ArgumentNullException(nameof(topLevel));
        _initialDirectoryFromSettings = initialDirectoryFromSettings ?? throw new ArgumentNullException(nameof(initialDirectoryFromSettings));
        AboutWindowCommand = ReactiveCommand.CreateFromTask(AboutWindowCommandAction);
        BrowseInitialDirectoryCommand = ReactiveCommand.CreateFromTask(BrowseInitialDirectoryCommandAction);
        ThrowTheDiceCommand = ReactiveCommand.CreateFromTask(ThrowTheDiceCommandAction);
    }
}