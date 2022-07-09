using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Dice.Avalonia
{
    /// <inheritdoc />
    public partial class AboutWindow : UserControl
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}