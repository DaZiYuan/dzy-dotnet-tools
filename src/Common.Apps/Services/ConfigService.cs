using Common.Helpers;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Apps.Services
{
    /// <summary>
    /// 配置文件读写
    /// </summary>
    public class ConfigService
    {
        private readonly Logger _logger;
        public string UserConfigDir { get; private set; }
        public string UserConfigFile { get; private set; }
        public string AppConfigDir { get; private set; }
        public string LogDir { get; private set; }

        public object? UserConfig { get; private set; }

        public ConfigService(Logger logger,InitServiceOption option)
        {
            _logger = logger;
            AppConfigDir = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\{option.AppName}\\";

            //卸载时不清除
            UserConfigDir = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\{option.AppName}\\UserData";
            UserConfigFile = Path.Combine(UserConfigDir, "userconfig.json");
            LogDir = $"{AppConfigDir}/Logs";
        }

        public async Task LoadUserConfig<T>()
        {
            await Task.Run(() =>
            {
                try
                {
                    UserConfig = JsonHelper.JsonDeserializeFromFile<T>(UserConfigFile);
                }
                catch (Exception ex)
                {
                    _logger.Warn(ex, "LoadUserConfig ex");
                }
            });
        }

        public async void SaveUserConfig(object config)
        {
            UserConfig = config;
            await Task.Run(() =>
            {
                try
                {
                    JsonHelper.JsonSerialize(config, UserConfigFile);
                }
                catch (Exception ex)
                {
                    _logger.Warn(ex, "SaveUserConfig ex");
                }
            });
        }
    }
}
