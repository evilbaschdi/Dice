using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Dice.Core;
using Dice.Core.Settings;
using EvilBaschdi.Avalonia.Core;
using EvilBaschdi.Core.AppHelpers;
using EvilBaschdi.Core.Extensions;
using EvilBaschdi.Core.Internal;

namespace Dice.Avalonia
{
    /// <inheritdoc />
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, int> _pathClickCounter = new();
        private IDicePath _dicePath;
        private string _initialDirectory;
        private IInitialDirectoryFromSettings _initialDirectoryFromSettings;
        private string _path;
        private IProcessByPath _processByPath;

        /// <summary>
        ///     Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            IHandleOsDependentTitleBar handleOsDependentTitleBar = new HandleOsDependentTitleBar();
            handleOsDependentTitleBar.RunFor((this, HeaderPanel, MainPanel));

            _processByPath = new ProcessByPath();
            IFileListFromPath filePath = new FileListFromPath();
            _dicePath = new DicePath(filePath);

            IDiceSettingsFromJsonFile diceSettingsFromJsonFile = new DiceSettingsFromJsonFile();
            ICurrentDiceSettingsFromJsonFile currentDiceSettingsFromJsonFile = new CurrentDiceSettingsFromJsonFile();
            _initialDirectoryFromSettings = new InitialDirectoryFromSettings(diceSettingsFromJsonFile, currentDiceSettingsFromJsonFile);

            _initialDirectory = _initialDirectoryFromSettings.Value;
            ThrowTheDice.IsEnabled = !string.IsNullOrWhiteSpace(_initialDirectory) && Directory.Exists(_initialDirectory);
            InitialDirectory.Text = _initialDirectory;
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private async void ThrowTheDiceOnClick(object sender, RoutedEventArgs e)
        {
            // ReSharper disable once AsyncConverter.AsyncAwaitMayBeElidedHighlighting

            await RunDiceAsync().ConfigureAwait(true);
        }

        private async Task RunDiceAsync()
        {
            //await MessageBox.Show(this, "Test", "Test title", MessageBox.MessageBoxButtons.YesNoCancel);

            var task = Task<string>.Factory.StartNew(_dicePath.ValueFor(_initialDirectoryFromSettings.Value));
            await task.ConfigureAwait(true);
            _path = task.Result;

            if (!_pathClickCounter.ContainsKey(_path))
            {
                _pathClickCounter.Add(_path, 1);
            }

            var clicks = _pathClickCounter[_path];
            _pathClickCounter[_path] += 1;

            var intToWord = clicks == 1 ? "once" : $"{clicks.ToWords()} times";

            ThrowTheDiceContent.Text = $"'{_path}'{Environment.NewLine}(diced {intToWord}){Environment.NewLine}[roll the dice again]";
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private void InitialDirectoryOnLostFocus(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(InitialDirectory.Text))
            {
                return;
            }

            _initialDirectoryFromSettings.Value = InitialDirectory.Text;
            //Load();
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private async void BrowseClick(object sender, RoutedEventArgs e)
        {
            var browser = new OpenFolderDialog
                          {
                              Title = "Choose Directory to dice",
                              Directory = _initialDirectory
                          };
            var result = await browser.ShowAsync(this);
            InitialDirectory.Text = result;
            _initialDirectoryFromSettings.Value = result;
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private void ThrowTheDiceOnPointerPressed(object sender, PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            var propertiesPointerUpdateKind = e.GetCurrentPoint(this).Properties.PointerUpdateKind;
            if (propertiesPointerUpdateKind == PointerUpdateKind.RightButtonPressed)
            {
                try
                {
                    var path = _path ?? _initialDirectory;
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
}