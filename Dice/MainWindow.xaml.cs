using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shell;
using Dice.Core;
using Dice.Internal;
using Dice.Properties;
using EvilBaschdi.Core.Application;
using EvilBaschdi.Core.Browsers;
using EvilBaschdi.Core.DirectoryExtensions;
using EvilBaschdi.Core.Threading;
using EvilBaschdi.Core.Wpf;
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
        private readonly IDialogService _dialogService;
        private readonly IMetroStyle _style;
        private readonly IDicePath _dicePath;

        private string _initialDirectory;
        private int _overrideProtection;
        private string _path;

        /// <inheritdoc />
        public MainWindow()
        {
            InitializeComponent();

            _appSettings = new AppSettings();
            ISettings coreSettings = new CoreSettings(Settings.Default);
            IThemeManagerHelper themeManagerHelper = new ThemeManagerHelper();
            IMultiThreadingHelper multiThreadingHelper = new MultiThreadingHelper();
            IFilePath filePath = new FilePath(multiThreadingHelper);
            _dicePath = new DicePath(filePath);
            _style = new MetroStyle(this, Accent, ThemeSwitch, coreSettings, themeManagerHelper);
            _style.Load(true);
            var linkerTime = Assembly.GetExecutingAssembly().GetLinkerTime();
            LinkerTime.Content = linkerTime.ToString(CultureInfo.InvariantCulture);


            _dialogService = new DialogService(this);

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
            if (Directory.Exists(InitialDirectory.Text))
            {
                _appSettings.InitialDirectory = InitialDirectory.Text;
                Load();
            }
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

        #region Flyout

        private void ToggleSettingsFlyoutClick(object sender, RoutedEventArgs e)
        {
            ToggleFlyout(0);
        }

        private void ToggleFlyout(int index, bool stayOpen = false)
        {
            var activeFlyout = (Flyout) Flyouts.Items[index];
            if (activeFlyout == null)
            {
                return;
            }

            foreach (
                var nonactiveFlyout in
                Flyouts.Items.Cast<Flyout>()
                       .Where(nonactiveFlyout => nonactiveFlyout.IsOpen && nonactiveFlyout.Name != activeFlyout.Name))
            {
                nonactiveFlyout.IsOpen = false;
            }

            if (activeFlyout.IsOpen && stayOpen)
            {
                activeFlyout.IsOpen = true;
            }
            else
            {
                activeFlyout.IsOpen = !activeFlyout.IsOpen;
            }
        }

        private void GetReferencedVersions(object sender, MouseButtonEventArgs e)
        {
            var referencedAssemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Where(ra => ra.Name.Equals("MahApps.Metro") || ra.Name.Equals("EvilBaschdi.Core"));
            //EvilBaschdi.Core
            //MahApps.Metro
            //MahApps.Metro.IconPacks

            var versionStringBuilder = new StringBuilder();

            foreach (var referencedAssembly in referencedAssemblies)
            {
                versionStringBuilder.AppendLine($"{referencedAssembly.Name}: {referencedAssembly.Version}");
            }


            _dialogService.ShowMessage("Referenced Versions", versionStringBuilder.ToString());
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