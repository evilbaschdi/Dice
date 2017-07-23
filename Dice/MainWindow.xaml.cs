using System;
using System.Collections.Generic;
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
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly IAppSettings _appSettings;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ISettings _coreSettings;

        private readonly IDialogService _dialogService;
        private readonly IFilePath _folderPath;
        private readonly IMetroStyle _style;
        private IList<string> _folderList;
        private string _initialDirectory;
        private int _overrideProtection;

        private string _path;
        //private readonly List<Debug> _debugList;

        public MainWindow()
        {
            InitializeComponent();
            _appSettings = new AppSettings();
            _coreSettings = new CoreSettings(Settings.Default);
            var themeManagerHelper = new ThemeManagerHelper();
            _style = new MetroStyle(this, Accent, ThemeSwitch, _coreSettings, themeManagerHelper);
            _style.Load(true);
            var linkerTime = Assembly.GetExecutingAssembly().GetLinkerTime();
            LinkerTime.Content = linkerTime.ToString(CultureInfo.InvariantCulture);
            var multiThreadingHelper = new MultiThreadingHelper();
            _folderPath = new FilePath(multiThreadingHelper);
            _dialogService = new DialogService(this);

            // -- DEBUG --
            //_debugList = new List<Debug>();
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

            // -- DEBUG --
            //foreach(var pair in _debugList.OrderByDescending(p => p.Calls))
            //{
            //    File.AppendAllText(@"C:\temp\debug.txt", $"{pair.Path}: {pair.Calls}{Environment.NewLine}");
            //}
            //Process.Start(@"C:\temp\debug.txt");
        }


        //private async void buttonAsync_Click(object sender, RoutedEventArgs e)
        //{
        //    buttonAsync.IsEnabled = false;

        //    var slowTask = Task<string>.Factory.StartNew(() => SlowDude());

        //    textBoxResults.Text += "Doing other things while waiting for that slow dude...\r\n";

        //    await slowTask;

        //    textBoxResults.Text += slowTask.Result.ToString();

        //    buttonAsync.IsEnabled = true;
        //}

        //private string SlowDude()
        //{
        //    Thread.Sleep(2000);
        //    return "Ta-dam! Here I am!\r\n";
        //}


        private async void ThrowTheDiceOnClick(object sender, RoutedEventArgs e)
        {
            // ReSharper disable once AsyncConverter.AsyncAwaitMayBeElidedHighlighting
            await RunDiceAsync().ConfigureAwait(true);
        }

        private async Task RunDiceAsync()
        {
            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Indeterminate;
            Cursor = Cursors.Wait;

            var task = Task<string>.Factory.StartNew(Dice);
            await task.ConfigureAwait(true);

            ThrowTheDiceContent.Text = task.Result;

            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
            Cursor = Cursors.Arrow;
        }

        private string Dice()
        {
            _folderList = _folderPath.GetSubdirectoriesContainingOnlyFiles(_initialDirectory).ToList();


            var index = GenerateRandomNumber(0, _folderList.Count);


            _path = _folderList[index];

            return $"'{_path}'{Environment.NewLine}{Environment.NewLine}[click to dice again]";
        }

        /// <summary>
        ///     because random is not random enough.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private static int GenerateRandomNumber(int min, int max)
        {
            var result = RandomGenerator.Next();
            return result % max + min;
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
            var routedEventArgs = e as RoutedEventArgs;
            if (routedEventArgs != null)
            {
                _style.SetTheme(sender, routedEventArgs);
            }
            else
            {
                _style.SetTheme(sender);
            }
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