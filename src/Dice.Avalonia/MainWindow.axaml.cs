using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Dice.Avalonia.ViewModels;
using Dice.Core;
using Dice.Core.Settings;
using EvilBaschdi.Core.AppHelpers;
using EvilBaschdi.Core.Avalonia.DependencyInjection;
using EvilBaschdi.Core.Settings.ByMachineAndUser;
using FluentAvalonia.UI.Windowing;

namespace Dice.Avalonia;

/// <inheritdoc />
public partial class MainWindow : FAAppWindow
{
    private string _initialDirectory;
    private IInitialDirectoryFromSettings _initialDirectoryFromSettings;
    private IProcessByPath _processByPath;

    /// <summary>
    ///     Constructor
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();

        var topLevel = ApplicationServices.GetRequiredService<ITopLevel>();
        topLevel.Value = TopLevel.GetTopLevel(this);
        // Here you can reference various services like Clipboard or StorageProvider from topLevel instance.

        Load();
    }

    private void Load()
    {
        _processByPath = new ProcessByPath();

        IAppSettingsFromJsonFile appSettingsFromJsonFile = new AppSettingsFromJsonFile();
        IAppSettingsFromJsonFileByMachineAndUser appSettingsFromJsonFileByMachineAndUser = new AppSettingsFromJsonFileByMachineAndUser();
        IAppSettingByKey appSettingByKey = new AppSettingByKey(appSettingsFromJsonFile, appSettingsFromJsonFileByMachineAndUser);

        _initialDirectoryFromSettings = new InitialDirectoryFromSettings(appSettingByKey);

        _initialDirectory = _initialDirectoryFromSettings.Value;
        ThrowTheDice.IsEnabled = !string.IsNullOrWhiteSpace(_initialDirectory) && Directory.Exists(_initialDirectory);
        InitialDirectory.Text = _initialDirectory;
    }

    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    // ReSharper disable once UnusedMember.Local
    private void InitialDirectoryOnLostFocus(object sender, RoutedEventArgs e)
    {
        if (!Directory.Exists(InitialDirectory.Text))
        {
            return;
        }

        _initialDirectoryFromSettings.Value = InitialDirectory.Text;
    }

    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    // ReSharper disable once UnusedMember.Local
    private async void BrowseClick(object sender, RoutedEventArgs e)
    {
        var storageProvider = GetTopLevel().StorageProvider;

        var folderPickerOpenOptions = new FolderPickerOpenOptions
                                      {
                                          Title = "Choose Directory to dice",
                                          //SuggestedStartLocation = 
                                          AllowMultiple = false
                                      };
        var folderPicker = await storageProvider.OpenFolderPickerAsync(folderPickerOpenOptions);

        var storageFolder = folderPicker.FirstOrDefault();
        var fullPath = FullPathOrName(storageFolder);
        InitialDirectory.Text = fullPath;
        _initialDirectoryFromSettings.Value = fullPath;
    }

    private static string FullPathOrName(IStorageItem item) => item is null ? "(null)" : item.Path.LocalPath;

    //return item.TryGetUri(out var uri) ? uri.LocalPath : item.Name;
    private TopLevel GetTopLevel() => TopLevel.GetTopLevel(this);
    // Here you can reference various services like Clipboard or StorageProvider from topLevel instance. as TopLevel ?? throw new NullReferenceException("Invalid Owner");

    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    // ReSharper disable once UnusedMember.Local
    private void ThrowTheDiceOnPointerPressed(object sender, PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        var dicedPath = ApplicationServices.GetRequiredService<IRollTheDiceResultPath>()?.Value;

        var propertiesPointerUpdateKind = e.GetCurrentPoint(this).Properties.PointerUpdateKind;
        if (propertiesPointerUpdateKind == PointerUpdateKind.RightButtonPressed)
        {
            try
            {
                var path = dicedPath ?? _initialDirectory;
                if (!string.IsNullOrWhiteSpace(path))
                {
                    _processByPath.RunFor(path);
                }
            }
            catch (Exception)
            {
                //this.ShowMessageAsync(e.GetType().ToString(), e.Message);
            }
        }
    }
}