using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using VamTouch.Core.Models;

namespace VamTouch.Core.Services
{
    public class FileManager : IFileManager
    {
        private string _varDirectory;
        private readonly IConfigManager _configManager;

        public FileManager(IConfigManager configManager)
        {
            _configManager = configManager;
            InitializeDirectory();
        }

        private void InitializeDirectory()
        {
            if (_configManager.ConfigExists())
            {
                var config = _configManager.LoadConfig().GetAwaiter().GetResult();
                _varDirectory = config.VarDirectoryPath;
            }
        }

        public async Task<bool> SetVarDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                return false;
            }

            _varDirectory = directoryPath;

            // 更新配置
            var config = await _configManager.LoadConfig();
            config.VarDirectoryPath = directoryPath;
            config.LastAccessed = DateTime.Now;
            await _configManager.SaveConfig(config);

            return true;
        }

        public string GetVarDirectory()
        {
            return _varDirectory;
        }

        public async Task<List<string>> ScanVarFiles()
        {
            if (string.IsNullOrEmpty(_varDirectory) || !Directory.Exists(_varDirectory))
            {
                return new List<string>();
            }

            // 查找所有.var文件
            return await Task.Run(() => 
                Directory.EnumerateFiles(_varDirectory, "*.var", SearchOption.AllDirectories).ToList()
            );
        }

        public async Task<bool> OpenVarFileLocation(VarData varData)
        {
            if (varData == null || string.IsNullOrEmpty(varData.Location?.Directory))
            {
                return false;
            }

            string directory = varData.Location.Directory;
            if (!Directory.Exists(directory))
            {
                return false;
            }

            await Task.Run(() =>
            {
                // 根据操作系统打开文件资源管理器
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Process.Start("explorer.exe", directory);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", directory);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", directory);
                }
            });

            return true;
        }

        public async Task<bool> DeleteVarFile(VarData varData)
        {
            if (varData == null || string.IsNullOrEmpty(varData.OriginInfo?.Filename))
            {
                return false;
            }

            string filePath = Path.Combine(varData.Location.Directory, varData.OriginInfo.Filename);
            if (!File.Exists(filePath))
            {
                return false;
            }

            try
            {
                await Task.Run(() => File.Delete(filePath));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> OpenFileLocation(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return false;
            }

            string directory = Path.GetDirectoryName(filePath);
            if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory))
            {
                return false;
            }

            await Task.Run(() =>
            {
                // 根据操作系统打开文件资源管理器
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Process.Start("explorer.exe", directory);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", directory);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", directory);
                }
            });

            return true;
        }

        public async Task<bool> DeleteVarFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return false;
            }

            try
            {
                await Task.Run(() => File.Delete(filePath));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
} 
 