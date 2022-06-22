using System.Security.Cryptography;
using EvilBaschdi.Core;
using EvilBaschdi.Core.Internal;
using EvilBaschdi.Settings;

namespace Dice.Core;

/// <inheritdoc />
public class DicePath : IDicePath
{
    private readonly IFileListFromPath _filePath;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="filePath"></param>
    public DicePath(IFileListFromPath filePath)
    {
        _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
    }

    /// <inheritdoc />
    public Func<string> ValueFor(string initialDirectory)
    {
        if (initialDirectory == null)
        {
            throw new ArgumentNullException(nameof(initialDirectory));
        }

        return () =>
               {
                   var folderList = _filePath.GetSubdirectoriesContainingOnlyFiles(initialDirectory).ToList();
                   var index = RandomNumberGenerator.GetInt32(0, folderList.Count);

                   return folderList[index];
               };
    }
}

/// <inheritdoc />
public interface IInitialDirectoryFromSettings : IString
{
}

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

    /// <inheritdoc />
    public string? Value
    {
        get
        {
            var configuration = _diceSettingsFromJsonFile.Value;
            //todo:per device / user configuration
            var initialDirectory = configuration["InitialDirectory"];

            return initialDirectory;
        }
    }
}

/// <inheritdoc />
public interface IDiceSettingsFromJsonFile : ISettingsFromJsonFile
{
}

/// <inheritdoc cref="WritableSettingsFromJsonFile" />
public class DiceSettingsFromJsonFile : WritableSettingsFromJsonFile, IDiceSettingsFromJsonFile
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public DiceSettingsFromJsonFile()
        : base("Settings\\DiceSettings.json")
    {
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