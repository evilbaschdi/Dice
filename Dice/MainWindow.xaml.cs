using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shell;
using Dice.Core;
using Dice.Internal;
using Dice.Properties;
using EvilBaschdi.Core.Extensions;
using EvilBaschdi.Core.Internal;
using EvilBaschdi.CoreExtended.AppHelpers;
using EvilBaschdi.CoreExtended.Browsers;
using EvilBaschdi.CoreExtended.Metro;
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
        private readonly IApplicationStyle _style;
        private string _initialDirectory;
        private int _overrideProtection;
        private string _path;

        /// <inheritdoc />
        public MainWindow()
        {
            InitializeComponent();


            IAppSettingsBase appSettingsBase = new AppSettingsBase(Settings.Default);
            IApplicationStyleSettings coreSettings = new ApplicationStyleSettings(appSettingsBase);
            IThemeManagerHelper themeManagerHelper = new ThemeManagerHelper();
            IMultiThreading multiThreadingHelper = new MultiThreading();
            IFileListFromPath filePath = new FileListFromPath(multiThreadingHelper);
            _screenShot = new ScreenShot();
            _appSettings = new AppSettings(appSettingsBase);
            _dicePath = new DicePath(filePath);
            _style = new ApplicationStyle(this, Accent, ThemeSwitch, coreSettings, themeManagerHelper);
            _style.Load(true);
            var linkerTime = Assembly.GetExecutingAssembly().GetLinkerTime();
            LinkerTime.Content = linkerTime.ToString(CultureInfo.InvariantCulture);


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

        #region Flyout

        private void ToggleSettingsFlyOutClick(object sender, RoutedEventArgs e)
        {
            ToggleFlyOut(0);
        }

        private void ToggleFlyOut(int index, bool stayOpen = false)
        {
            var activeFlyOut = (Flyout) Flyouts.Items[index];
            if (activeFlyOut == null)
            {
                return;
            }

            foreach (
                var inactiveFlyOut in
                Flyouts.Items.Cast<Flyout>()
                       .Where(item => item.IsOpen && item.Name != activeFlyOut.Name))
            {
                inactiveFlyOut.IsOpen = false;
            }

            if (activeFlyOut.IsOpen && stayOpen)
            {
                activeFlyOut.IsOpen = true;
            }
            else
            {
                activeFlyOut.IsOpen = !activeFlyOut.IsOpen;
            }
        }

        #endregion Flyout

        #region MetroStyle

        private void SaveStyleClick(object sender, RoutedEventArgs e)
        {
            if (_overrideProtection == 0)
            {
                return;
            }

            _style.SaveStyle();
        }

        private void Theme(object sender, EventArgs e)
        {
            if (_overrideProtection == 0)
            {
                return;
            }

            _style.SetTheme(sender);
        }

        private void AccentOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_overrideProtection == 0)
            {
                return;
            }

            _style.SetAccent(sender, e);
        }

        #endregion MetroStyle
    }
}