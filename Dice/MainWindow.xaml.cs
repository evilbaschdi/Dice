using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Dice.Core;
using Dice.Internal;
using Dice.Properties;
using EvilBaschdi.Core.Extensions;
using EvilBaschdi.Core.Internal;
using EvilBaschdi.CoreExtended;
using EvilBaschdi.CoreExtended.AppHelpers;
using EvilBaschdi.CoreExtended.Browsers;
using EvilBaschdi.CoreExtended.Controls.About;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

// ReSharper disable RedundantExtendsListEntry

namespace Dice
{
    /// <inheritdoc cref="MetroWindow" />
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly IAppSettings _appSettings;
        private readonly IDicePath _dicePath;
        private readonly Dictionary<string, int> _pathClickCounter = new Dictionary<string, int>();
        private readonly IProcessByPath _processByPath;
        private readonly IRoundCorners _roundCorners;
        private readonly IScreenShot _screenShot;
        private string _initialDirectory;
        private string _path;

        /// <inheritdoc />
        public MainWindow()
        {
            InitializeComponent();

            if (Settings.Default.UpgradeRequired)
            {
                Settings.Default.Upgrade();
                Settings.Default.UpgradeRequired = false;
                Settings.Default.Save();
            }

            IAppSettingsBase appSettingsBase = new AppSettingsBase(Settings.Default);
            IFileListFromPath filePath = new FileListFromPath();

            _screenShot = new ScreenShot();
            _appSettings = new AppSettings(appSettingsBase);
            _dicePath = new DicePath(filePath);
            _roundCorners = new RoundCorners();
            IApplicationStyle style = new ApplicationStyle(_roundCorners, true);
            style.Run();
            _processByPath = new ProcessByPath();

            Load();
        }

        private void Load()
        {
            ThrowTheDice.IsEnabled = !string.IsNullOrWhiteSpace(_appSettings.InitialDirectory) &&
                                     Directory.Exists(_appSettings.InitialDirectory);

            _initialDirectory = _appSettings.InitialDirectory;
            InitialDirectory.Text = _initialDirectory ?? string.Empty;

            ThrowTheDice.MouseRightButtonDown += ThrowTheDiceOnMouseRightButtonDown;
        }

        private void ThrowTheDiceOnMouseRightButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_path))
                    _processByPath.RunFor(_path);
                else if (!string.IsNullOrWhiteSpace(_initialDirectory))
                    _processByPath.RunFor(_initialDirectory);
                else
                    this.ShowMessageAsync("Error", "Path is empty. Please choose a path and roll the dice");
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

            if (!_pathClickCounter.ContainsKey(_path)) _pathClickCounter.Add(_path, 1);

            var clicks = _pathClickCounter[_path];
            _pathClickCounter[_path] += 1;

            var intToWord = clicks == 1 ? "once" : $"{clicks.ToWords()} times";

            ThrowTheDiceContent.Text =
                $"'{_path}'{Environment.NewLine}(diced {intToWord}){Environment.NewLine}[roll the dice again]";
        }

        private void InitialDirectoryOnLostFocus(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(InitialDirectory.Text)) return;

            _appSettings.InitialDirectory = InitialDirectory.Text;
            Load();
        }


        private void BrowseClick(object sender, RoutedEventArgs e)
        {
            var browser = new ExplorerFolderBrowser
            {
                SelectedPath = _initialDirectory
            };
            browser.ShowDialog();
            _appSettings.InitialDirectory = browser.SelectedPath;
            Load();
        }

        private void ScreenShotClick(object sender, RoutedEventArgs e)
        {
            var current = _screenShot.ValueFor(this);
            _screenShot.SaveToClipboard(current);
        }

        private void AboutWindowClick(object sender, RoutedEventArgs e)
        {
            var assembly = typeof(MainWindow).Assembly;
            IAboutContent aboutWindowContent =
                new AboutContent(assembly, $@"{AppDomain.CurrentDomain.BaseDirectory}\dice.png");

            var aboutWindow = new AboutWindow
            {
                DataContext = new AboutViewModel(aboutWindowContent, _roundCorners)
            };

            aboutWindow.ShowDialog();
        }
    }
}