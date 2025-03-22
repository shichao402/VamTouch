using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using VamTouch.Core.Models;

namespace VamTouch.Core.Services
{
    public class VarDataManager : IVarDataManager
    {
        private readonly IVarParser _varParser;
        private readonly IFileManager _fileManager;
        private readonly IConfigManager _configManager;
        private List<VarData> _varDataList = new List<VarData>();

        private readonly string _varDataStatePath;

        public VarDataManager(IVarParser varParser, IFileManager fileManager, IConfigManager configManager)
        {
            _varParser = varParser;
            _fileManager = fileManager;
            _configManager = configManager;

            // VarData状态文件放在与配置文件相同的目录中
            string configFilePath = _configManager.GetConfigFilePath();
            string configDir = Path.GetDirectoryName(configFilePath);
            _varDataStatePath = Path.Combine(configDir, "vardata.json");
        }

        public async Task<List<VarData>> LoadVarData(List<string> varFilePaths)
        {
            _varDataList.Clear();

            foreach (var filePath in varFilePaths)
            {
                var varData = await _varParser.ParseVarFile(filePath);
                if (varData != null)
                {
                    _varDataList.Add(varData);
                }
            }

            // 保存状态
            await SaveVarDataState();

            return _varDataList;
        }

        public async Task<bool> RefreshVarData()
        {
            try
            {
                // 获取所有VAR文件
                var varFiles = await _fileManager.ScanVarFiles();
                if (varFiles.Count == 0)
                {
                    return false;
                }

                // 更新现有数据
                var existingFilenames = _varDataList.Select(v => v.OriginInfo?.Filename).ToList();
                
                // 移除不存在的文件
                _varDataList.RemoveAll(v => 
                    !varFiles.Any(f => Path.GetFileName(f) == v.OriginInfo?.Filename));
                
                // 添加新文件
                foreach (var filePath in varFiles)
                {
                    string filename = Path.GetFileName(filePath);
                    if (!existingFilenames.Contains(filename))
                    {
                        var varData = await _varParser.ParseVarFile(filePath);
                        if (varData != null)
                        {
                            _varDataList.Add(varData);
                        }
                    }
                }

                // 保存状态
                await SaveVarDataState();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<VarData> GetAllVarData()
        {
            return _varDataList.ToList();
        }

        public List<VarData> GetVarDataByCategory(CategoryType category)
        {
            return _varDataList.Where(v => v.Category == category).ToList();
        }

        public List<VarData> GetVarDataByCreator(string creator)
        {
            return _varDataList.Where(v => 
                string.Equals(v.Creator, creator, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public async Task<bool> SaveVarDataState()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string json = JsonSerializer.Serialize(_varDataList, options);
                await File.WriteAllTextAsync(_varDataStatePath, json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> LoadVarDataState()
        {
            if (!File.Exists(_varDataStatePath))
            {
                return false;
            }

            try
            {
                string json = await File.ReadAllTextAsync(_varDataStatePath);
                _varDataList = JsonSerializer.Deserialize<List<VarData>>(json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveVarData(string id)
        {
            try
            {
                // 找到要删除的VAR数据
                var varData = _varDataList.FirstOrDefault(v => v.Id == id);
                if (varData == null)
                {
                    return false;
                }
                
                // 从列表中移除
                _varDataList.Remove(varData);
                
                // 保存状态
                await SaveVarDataState();
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
} 