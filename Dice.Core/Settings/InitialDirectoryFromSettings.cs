namespace Dice.Core.Settings;

/// <inheritdoc />
public class InitialDirectoryFromSettings : IInitialDirectoryFromSettings
{
    private readonly IDiceSettingsFromJsonFile _diceSettingsFromJsonFile;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="diceSettingsFromJsonFile"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public InitialDirectoryFromSettings(IDiceSettingsFromJsonFile diceSettingsFromJsonFile)
    {
        _diceSettingsFromJsonFile = diceSettingsFromJsonFile ?? throw new ArgumentNullException(nameof(diceSettingsFromJsonFile));
    }

    /// <inheritdoc cref="string" />
    public string? Value
    {
        get
        {
            var configuration = _diceSettingsFromJsonFile.Value;

            //todo maybe  per device / user configuration?
            var initialDirectory = configuration?["InitialDirectory"];

            return initialDirectory;
        }
        set
        {
            if (value == null)
            {
                return;
            }

            var configuration = _diceSettingsFromJsonFile.Value;
            configuration["InitialDirectory"] = value;
        }
    }
}

///// <inheritdoc />
//public interface ICurrentDiceSettingsFromJsonFile : ISettingsFromJsonFile
//{
//}

///// <inheritdoc cref="WritableSettingsFromJsonFile" />
//public class CurrentDiceSettingsFromJsonFile : WritableSettingsFromJsonFile, ICurrentDiceSettingsFromJsonFile
//{
//    /// <summary>
//    ///     Constructor
//    /// </summary>
//    public CurrentDiceSettingsFromJsonFile()
//        : base($"Settings\\DiceSettings.{Environment.MachineName}.{Environment.UserName}.json")
//    {
//    }
//}