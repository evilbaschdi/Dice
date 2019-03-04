using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shell;
using Dice.Core;
using Dice.Internal;
using Dice.Properties;
using EvilBaschdi.Core.Internal;
using EvilBaschdi.CoreExtended.AppHelpers;
using EvilBaschdi.CoreExtended.Browsers;
using EvilBaschdi.CoreExtended.Metro;
using EvilBaschdi.CoreExtended.Mvvm;
using EvilBaschdi.CoreExtended.Mvvm.View;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel;
using MahApps.Metro.Controls;

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
        private readonly IScreenShot _screenShot;
        private readonly IThemeManagerHelper _themeManagerHelper;
        private string _initialDirectory;
        private int _overrideProtection;
        private string _path;
        private AboutWindow _aboutWindow;

        /// <inheritdoc />
        public MainWindow()
        {
            InitializeComponent();


            IAppSettingsBase appSettingsBase = new AppSettingsBase(Settings.Default);
            _themeManagerHelper = new ThemeManagerHelper();
            IMultiThreading multiThreadingHelper = new MultiThreading();
            IFileListFromPath filePath = new FileListFromPath(multiThreadingHelper);
            _screenShot = new ScreenShot();
            _appSettings = new AppSettings(appSettingsBase);
            _dicePath = new DicePath(filePath);
            //_style = new ApplicationStyle(this, Accent, ThemeSwitch, coreSettings, themeManagerHelper);
            //_style.Load(true);
            _aboutWindow = new AboutWindow();
            var assembly = typeof(MainWindow).Assembly;

            IAboutWindowContent aboutWindowContent = new AboutWindowContent(assembly, $@"{AppDomain.CurrentDomain.BaseDirectory}\dice.png");
            _aboutWindow.DataContext = new AboutViewModel(aboutWindowContent, _themeManagerHelper);

            foreach (Window currentWindow in Application.Current.Windows)
            {
                _themeManagerHelper.SetSystemColorTheme(currentWindow);
            }

            Load();
        }

        private void Load()
        {
            ThrowTheDice.IsEnabled = !string.IsNullOrWhiteSpace(_appSettings.InitialDirectory) && Directory.Exists(_appSettings.InitialDirectory);

            _initialDirectory = _appSettings.InitialDirectory;
            InitialDirectory.Text = _initialDirectory;

            ThrowTheDice.MouseRightButtonDown += ThrowTheDiceOnMouseRightButtonDown;
            _overrideProtection = 1;
        }

        private void ThrowTheDiceOnMouseRightButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            Process.Start(_path);
        }

        private async void ThrowTheDiceOnClick(object sender, RoutedEventArgs e)
        {
            // ReSharper disable once AsyncConverter.AsyncAwaitMayBeElidedHighlighting
            await RunDiceAsync().ConfigureAwait(true);
        }

        private async void ThumbButtonInfoBrowseClick(object sender, EventArgs e)
        {
            // ReSharper disable once AsyncConverter.AsyncAwaitMayBeElidedHighlighting
            await RunDiceAsync().ConfigureAwait(true);
        }

        private async Task RunDiceAsync()
        {
            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Indeterminate;
            Cursor = Cursors.Wait;

            var task = Task<string>.Factory.StartNew(_dicePath.ValueFor(_initialDirectory));
            await task.ConfigureAwait(true);
            _path = task.Result;
            ThrowTheDiceContent.Text = $"'{_path}'{Environment.NewLine}{Environment.NewLine}[click to dice again]";

            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
            Cursor = Cursors.Arrow;
        }


        private void InitialDirectoryOnLostFocus(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(InitialDirectory.Text))
            {
                return;
            }

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
            _aboutWindow.Show();
        }
    }
}