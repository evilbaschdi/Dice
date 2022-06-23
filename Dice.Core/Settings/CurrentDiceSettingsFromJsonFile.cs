using EvilBaschdi.Settings;

namespace Dice.Core.Settings;

/// <inheritdoc cref="WritableSettingsFromJsonFile" />
public class CurrentDiceSettingsFromJsonFile : WritableSettingsFromJsonFile, ICurrentDiceSettingsFromJsonFile
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public CurrentDiceSettingsFromJsonFile()
        : base($"Settings\\DiceSettings.{Environment.MachineName}.{Environment.UserName}.json", true)
    {
    }
}