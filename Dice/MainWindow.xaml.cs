using System.IO;
using System.Windows;
using System.Windows.Input;
using Dice.Core;
using Dice.Core.Settings;
using EvilBaschdi.Core.AppHelpers;
using EvilBaschdi.Core.Extensions;
using EvilBaschdi.Core.Internal;
using EvilBaschdi.CoreExtended;
using EvilBaschdi.CoreExtended.AppHelpers;
using EvilBaschdi.CoreExtended.Browsers;
using EvilBaschdi.CoreExtended.Controls.About;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

// ReSharper disable RedundantExtendsListEntry

namespace Dice;

/// <inheritdoc cref="MetroWindow" />
/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : MetroWindow
{
    private readonly Dictionary<string, int> _pathClickCounter = new();
    private IDicePath _dicePath;
    private string _initialDirectory;
    private IInitialDirectoryFromSettings _initialDirectoryFromSettings;
    private string _path;
    private IProcessByPath _processByPath;
    private IScreenShot _screenShot;

    /// <inheritdoc />
    public MainWindow()
    {
        InitializeComponent();
        Load();
    }

    private void Load()
    {
        _screenShot = new ScreenShot();
        _processByPath = new ProcessByPath();
        IFileListFromPath filePath = new FileListFromPath();
        _dicePath = new DicePath(filePath);

        IDiceSettingsFromJsonFile diceSettingsFromJsonFile = new DiceSettingsFromJsonFile();
        _initialDirectoryFromSettings = new InitialDirectoryFromSettings(diceSettingsFromJsonFile);

        IRoundCorners roundCorners = new RoundCorners();
        IApplicationStyle style = new ApplicationStyle(roundCorners, true);
        style.Run();

        _initialDirectory = _initialDirectoryFromSettings.Value;
        ThrowTheDice.SetCurrentValue(IsEnabledProperty, !string.IsNullOrWhiteSpace(_initialDirectory) && Directory.Exists(_initialDirectory));
        InitialDirectory.SetCurrentValue(System.Windows.Controls.TextBox.TextProperty, _initialDirectory ?? string.Empty);

        ThrowTheDice.MouseRightButtonDown += ThrowTheDiceOnMouseRightButtonDown;
    }

    private void ThrowTheDiceOnMouseRightButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
    {
        try
        {
            var path = _path ?? _initialDirectory;
            if (!string.IsNullOrWhiteSpace(path))
            {
                _processByPath.RunFor(path);
            }
            else
            {
                this.ShowMessageAsync("Error", "Path is empty. Please choose a path and roll the dice");
            }
        }
        catch (Exception e)
        {
            this.ShowMessageAsync(e.GetType().ToString(), e.Message);
        }
    }

    private async void ThrowTheDiceOnClick(object sender, RoutedEventArgs e)
    {
        // ReSharper disable once AsyncConverter.AsyncAwaitMayBeElidedHighlighting
        await RunDiceAsync().ConfigureAwait(true);
    }

    private async Task RunDiceAsync()
    {
        var task = Task<string>.Factory.StartNew(_dicePath.ValueFor(_initialDirectory));
        await task.ConfigureAwait(true);
        _path = task.Result;

        if (!_pathClickCounter.ContainsKey(_path))
        {
            _pathClickCounter.Add(_path, 1);
        }

        var clicks = _pathClickCounter[_path];
        _pathClickCounter[_path] += 1;

        var intToWord = clicks == 1 ? "once" : $"{clicks.ToWords()} times";

        ThrowTheDiceContent.SetCurrentValue(System.Windows.Controls.TextBlock.TextProperty,
            $"'{_path}'{Environment.NewLine}(diced {intToWord}){Environment.NewLine}[roll the dice again]");
    }

    private void InitialDirectoryOnLostFocus(object sender, RoutedEventArgs e)
    {
        if (!Directory.Exists(InitialDirectory.Text))
        {
            return;
        }

        _initialDirectoryFromSettings.Value = InitialDirectory.Text;
        Load();
    }

    private void BrowseClick(object sender, RoutedEventArgs e)
    {
        var browser = new ExplorerFolderBrowser
                      {
                          SelectedPath = _initialDirectory
                      };
        browser.ShowDialog();
        _initialDirectoryFromSettings.Value = browser.SelectedPath;
        Load();
    }

    private void ScreenShotClick(object sender, RoutedEventArgs e)
    {
        var current = _screenShot.ValueFor(this);
        _screenShot.SaveToClipboard(current);
    }

    private void AboutWindowClick(object sender, RoutedEventArgs e)
    {
        ICurrentAssembly currentAssembly = new CurrentAssembly();
        IAboutContent aboutContent = new AboutContent(currentAssembly);
        IAboutModel aboutModel = new AboutViewModel(aboutContent);
        var aboutWindow = new AboutWindow(aboutModel);

        aboutWindow.ShowDialog();
    }
}