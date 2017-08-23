using Dice.Properties;

namespace Dice.Core
{
    /// <inheritdoc />
    public class AppSettings : IAppSettings
    {
        /// <inheritdoc />
        public string InitialDirectory
        {
            get => string.IsNullOrWhiteSpace(Settings.Default.InitialDirectory)
                ? ""
                : Settings.Default.InitialDirectory;
            set
            {
                Settings.Default.InitialDirectory = value;
                Settings.Default.Save();
            }
        }
    }
}