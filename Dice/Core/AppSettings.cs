using System;
using EvilBaschdi.CoreExtended.AppHelpers;

namespace Dice.Core
{
    /// <inheritdoc />
    public class AppSettings : IAppSettings
    {
        private readonly IAppSettingsBase _appSettingsBase;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="appSettingsBase"></param>
        public AppSettings(IAppSettingsBase appSettingsBase)
        {
            _appSettingsBase = appSettingsBase ?? throw new ArgumentNullException(nameof(appSettingsBase));
        }

        /// <inheritdoc />
        public string InitialDirectory
        {
            get => _appSettingsBase.Get("InitialDirectory", "");
            set => _appSettingsBase.Set("InitialDirectory", value);
        }
    }
}