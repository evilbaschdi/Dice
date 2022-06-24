using EvilBaschdi.Settings;

namespace Dice.Core.Settings;

/// <inheritdoc cref="WritableSettingsFromJsonFile" />
public class DiceSettingsFromJsonFile : WritableSettingsFromJsonFile, IDiceSettingsFromJsonFile
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public DiceSettingsFromJsonFile()
        : base("Settings/DiceSettings.json")
    {
    }
}