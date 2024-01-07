using Dice.Core;
using Dice.Core.Settings;
using EvilBaschdi.Core.AppHelpers;
using EvilBaschdi.Core.Internal;
using EvilBaschdi.Settings.ByMachineAndUser;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Dice.Avalonia.DependencyInjection;

/// <inheritdoc />
public class ConfigureCoreServices : IConfigureCoreServices
{
    /// <inheritdoc />
    public void RunFor([NotNull] IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IAppSettingByKey, AppSettingByKey>();
        services.AddSingleton<IAppSettingsFromJsonFile, AppSettingsFromJsonFile>();
        services.AddSingleton<IAppSettingsFromJsonFileByMachineAndUser, AppSettingsFromJsonFileByMachineAndUser>();

        services.AddSingleton<IDicePath, DicePath>();
        services.AddSingleton<IFileListFromPath, FileListFromPath>();

        services.AddSingleton<IInitialDirectoryFromSettings, InitialDirectoryFromSettings>();
        services.AddSingleton<IProcessByPath, ProcessByPath>();
        services.AddSingleton<IRollTheDice, RollTheDice>();
        services.AddSingleton<IRollTheDiceResultPath, RollTheDiceResultPath>();
    }
}