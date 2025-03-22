using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using VamTouch.Core.Models;

namespace VamTouch.Core.Services
{
    public class ConfigManager : IConfigManager
    {
        private readonly string _configFilePath;
        private readonly string _appVersionString = "1.0.0"; // 初始版本号

        public ConfigManager()
        {
            // 配置文件放在用户目录中的应用程序数据文件夹内
            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "VamTouch");
            
            // 确保目录存在
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }
            
            _configFilePath = Path.Combine(appDataPath, "config.json");
        }

        public async Task<AppConfig> LoadConfig()
        {
            if (!File.Exists(_configFilePath))
            {
                // 返回默认配置
                return new AppConfig
                {
                    VarDirectoryPath = string.Empty,
                    LastAccessed = DateTime.Now,
                    AppVersion = _appVersionString
                };
            }

            try
            {
                string json = await File.ReadAllTextAsync(_configFilePath);
                var config = JsonSerializer.Deserialize<AppConfig>(json);
                
                // 确保版本号存在
                if (string.IsNullOrEmpty(config.AppVersion))
                {
                    config.AppVersion = _appVersionString;
                }
                
                return config;
            }
            catch (Exception)
            {
                // 出错时返回默认配置
                return new AppConfig
                {
                    VarDirectoryPath = string.Empty,
                    LastAccessed = DateTime.Now,
                    AppVersion = _appVersionString
                };
            }
        }

        public async Task<bool> SaveConfig(AppConfig config)
        {
            try
            {
                // 更新访问时间
                config.LastAccessed = DateTime.Now;
                
                // 确保版本号存在
                if (string.IsNullOrEmpty(config.AppVersion))
                {
                    config.AppVersion = _appVersionString;
                }
                
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                
                string json = JsonSerializer.Serialize(config, options);
                await File.WriteAllTextAsync(_configFilePath, json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GetConfigFilePath()
        {
            return _configFilePath;
        }

        public bool ConfigExists()
        {
            return File.Exists(_configFilePath);
        }
    }
} 