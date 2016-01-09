using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Dice.Core;
using Dice.Internal;
using EvilBaschdi.Core.Application;
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
        private readonly IMetroStyle _style;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ISettings _coreSettings;

        private readonly IAppBasics _basics;
        private readonly IFilePath _folderPath;
        private string _initialDirectory;
        private int _overrideProtection;
        private IList<string> _folderList;
        private string _path;
        //private readonly List<Debug> _debugList;

        public MainWindow()
        {
            _basics = new AppBasics();
            _coreSettings = new CoreSettings();
            InitializeComponent();
            _style = new MetroStyle(this, Accent, Dark, Light, _coreSettings);
            _style.Load();
            _folderPath = new FilePath();
            // -- DEBUG --
            //_debugList = new List<Debug>();
            Load();
        }

        private void Load()
        {
            ThrowTheDice.IsEnabled = !string.IsNullOrWhiteSpace(Properties.Settings.Default.InitialDirectory) && Directory.Exists(Properties.Settings.Default.InitialDirectory);

            _initialDirectory = _basics.GetInitialDirectory();
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

        private void ThrowTheDiceOnClick(object sender, RoutedEventArgs e)
        {
            _folderList = _folderPath.GetSubdirectoriesContainingOnlyFiles(_initialDirectory);

            // -- DEBUG --
            //if (_debugList.Count == 0)
            //{
            //    foreach(var path in _folderList)
            //    {
            //        _debugList.Add(new Debug
            //        {
            //            Path = path,
            //            Calls = 0
            //        });
            //    }
            //}

            var index = GenerateRandomNumber(0, _folderList.Count - 1);

            _path = _folderList[index];

            // -- DEBUG --
            //foreach (var item in _debugList.Where(item => item.Path == _path))
            //{
            //    item.Calls++;
            //}

            ThrowTheDiceContent.Text = $"'{_path}'{Environment.NewLine}{Environment.NewLine}[click to dice again]";
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
            return result%max + min;
        }

        private void InitialDirectoryOnLostFocus(object sender, RoutedEventArgs e)
        {
            if(Directory.Exists(InitialDirectory.Text))
            {
                Properties.Settings.Default.InitialDirectory = InitialDirectory.Text;
                Properties.Settings.Default.Save();
                _initialDirectory = Properties.Settings.Default.InitialDirectory;
            }
            Load();
        }

        private void BrowseClick(object sender, RoutedEventArgs e)
        {
            _basics.BrowseFolder();
            InitialDirectory.Text = _basics.GetInitialDirectory();
            _initialDirectory = _basics.GetInitialDirectory();
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
            if(activeFlyout == null)
            {
                return;
            }

            foreach(
                var nonactiveFlyout in
                    Flyouts.Items.Cast<Flyout>()
                        .Where(nonactiveFlyout => nonactiveFlyout.IsOpen && nonactiveFlyout.Name != activeFlyout.Name))
            {
                nonactiveFlyout.IsOpen = false;
            }

            if(activeFlyout.IsOpen && stayOpen)
            {
                activeFlyout.IsOpen = true;
            }
            else
            {
                activeFlyout.IsOpen = !activeFlyout.IsOpen;
            }
        }

        #endregion Flyout

        #region MetroStyle

        private void SaveStyleClick(object sender, RoutedEventArgs e)
        {
            if(_overrideProtection == 0)
            {
                return;
            }
            _style.SaveStyle();
        }

        private void Theme(object sender, RoutedEventArgs e)
        {
            if(_overrideProtection == 0)
            {
                return;
            }
            _style.SetTheme(sender, e);
        }

        private void AccentOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(_overrideProtection == 0)
            {
                return;
            }
            _style.SetAccent(sender, e);
        }

        #endregion MetroStyle
    }
}