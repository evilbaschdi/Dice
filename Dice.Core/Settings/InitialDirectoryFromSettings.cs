namespace Dice.Core.Settings;

/// <inheritdoc />
public class InitialDirectoryFromSettings : IInitialDirectoryFromSettings
{
    private const string Key = "InitialDirectory";
    private readonly ICurrentDiceSettingsFromJsonFile _currentDiceSettingsFromJsonFile;
    private readonly IDiceSettingsFromJsonFile _diceSettingsFromJsonFile;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="diceSettingsFromJsonFile"></param>
    /// <param name="currentDiceSettingsFromJsonFile"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public InitialDirectoryFromSettings(IDiceSettingsFromJsonFile diceSettingsFromJsonFile, ICurrentDiceSettingsFromJsonFile currentDiceSettingsFromJsonFile)
    {
        _diceSettingsFromJsonFile = diceSettingsFromJsonFile ?? throw new ArgumentNullException(nameof(diceSettingsFromJsonFile));
        _currentDiceSettingsFromJsonFile = currentDiceSettingsFromJsonFile ?? throw new ArgumentNullException(nameof(currentDiceSettingsFromJsonFile));
    }

    /// <inheritdoc cref="string" />
    public string Value
    {
        get
        {
            var fallbackConfiguration = _diceSettingsFromJsonFile.Value;
            var currentConfiguration = _currentDiceSettingsFromJsonFile.Value;

            var fallbackInitialDirectory = fallbackConfiguration?[Key];
            var currentInitialDirectory = currentConfiguration?[Key];

            return !string.IsNullOrWhiteSpace(currentInitialDirectory)
                ? currentInitialDirectory
                : fallbackInitialDirectory;
        }
        set
        {
            if (value == null)
            {
                return;
            }

            var configuration = _currentDiceSettingsFromJsonFile.Value;
            configuration["InitialDirectory"] = value;
        }
    }
}