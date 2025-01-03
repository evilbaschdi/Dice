using Dice.Core;
using Dice.Core.Settings;
using EvilBaschdi.Core.AppHelpers;
using EvilBaschdi.Core.Internal;
using EvilBaschdi.Settings.ByMachineAndUser;
using Microsoft.Extensions.DependencyInjection;

namespace Dice.Avalonia.DependencyInjection;

/// <summary />
public static class ConfigureCoreServices
{
    /// <summary />
    public static void AddCoreServices(this IServiceCollection services)
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