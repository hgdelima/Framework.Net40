using Alcoa.Framework.Common.Enumerator;
using System;
using System.Configuration;
using System.Linq;

namespace Alcoa.Framework.Common
{
    /// <summary>
    /// Class that helps manipulate Web.Config and App.Config sections
    /// </summary>
    public static class ConfigHelper
    {
        /// <summary>
        /// Gets an AppSetting using an explicit string name
        /// </summary>
        public static string GetAppSetting(string appSettingName)
        {
            return ConfigurationManager.AppSettings[appSettingName] ?? string.Empty;
        }

        /// <summary>
        /// Gets an AppSetting using a common mapped AppSetting enum
        /// </summary>
        public static string GetAppSetting(CommonAppSettingKeyName appSettingName)
        {
            return ConfigurationManager.AppSettings[appSettingName.ToString()] ?? string.Empty;
        }

        /// <summary>
        /// Check if AppSettings section contains a specific key in Config
        /// </summary>
        public static bool ContainsAppKey(CommonAppSettingKeyName appSettingName)
        {
            return ConfigurationManager.AppSettings.AllKeys.Any(c => c.ToUpper().Equals(appSettingName.GetDescription().ToUpper()));
        }

        /// <summary>
        /// 
        /// </summary>
        public static T GetSection<T>(string name)
        {
            var configuration = Activator.CreateInstance<T>();

            try
            {
                configuration = (T)ConfigurationManager.GetSection(name);
            }
            catch (Exception ex)
            {
            }

            return configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public static LogLevel DefaultUnhandledExceptionLogLevel
        {
            get { return GetAppSetting(CommonAppSettingKeyName.DefaultUnhandledExceptionLevel).ToLevel(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static LogLevel ServiceEntryErrorLogLogLevel
        {
            get
            {
                var level = LogLevel.Off;
                if (ContainsAppKey(CommonAppSettingKeyName.ServiceEntryErrorLogLevel))
                    level = GetAppSetting(CommonAppSettingKeyName.ServiceEntryErrorLogLevel).ToLevel();

                return level;
            }
        }
    }
}
