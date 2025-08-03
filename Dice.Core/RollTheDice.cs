using Dice.Core.Settings;
using EvilBaschdi.Core.Extensions;
using JetBrains.Annotations;

namespace Dice.Core;

/// <inheritdoc />
public class RollTheDice : IRollTheDice
{
    private readonly IDicePath _dicePath;
    private readonly IRollTheDiceResultPath _rollTheDiceResultPath;
    private readonly IInitialDirectoryFromSettings _initialDirectoryFromSettings;
    private readonly Dictionary<string, int> _pathClickCounter = new();

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="dicePath"></param>
    /// <param name="rollTheDiceResultPath"></param>
    /// <param name="initialDirectoryFromSettings"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public RollTheDice([NotNull] IDicePath dicePath,
                       [NotNull] IRollTheDiceResultPath rollTheDiceResultPath,
                       [NotNull] IInitialDirectoryFromSettings initialDirectoryFromSettings)
    {
        _dicePath = dicePath ?? throw new ArgumentNullException(nameof(dicePath));
        _rollTheDiceResultPath = rollTheDiceResultPath ?? throw new ArgumentNullException(nameof(rollTheDiceResultPath));
        _initialDirectoryFromSettings = initialDirectoryFromSettings ?? throw new ArgumentNullException(nameof(initialDirectoryFromSettings));
    }

    /// <inheritdoc />
    public async Task<string> ValueAsync()
    {
        var path = await _dicePath.ValueForAsync(_initialDirectoryFromSettings.Value);

        _pathClickCounter.TryAdd(path, 1);

        var clicks = _pathClickCounter[path];
        _pathClickCounter[path] += 1;

        var intToWord = clicks == 1 ? "once" : $"{clicks.ToWords()} times";

        _rollTheDiceResultPath.Value = path;

        return $"'{path}'{Environment.NewLine}(diced {intToWord}){Environment.NewLine}[roll the dice again]";
    }
}