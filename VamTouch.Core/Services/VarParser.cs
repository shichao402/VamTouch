using System;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Threading.Tasks;
using VamTouch.Core.Models;

namespace VamTouch.Core.Services
{
    public class VarParser : IVarParser
    {
        public async Task<VarData> ParseVarFile(string varFilePath)
        {
            if (!File.Exists(varFilePath))
            {
                return null;
            }

            try
            {
                // 提取meta.json数据
                var meta = await ExtractMetaJson(varFilePath);
                if (meta == null)
                {
                    return null;
                }

                // 创建VarData对象
                var varData = new VarData
                {
                    Creator = meta.CreatorName ?? "Unknown",
                    PackageName = meta.PackageName ?? Path.GetFileNameWithoutExtension(varFilePath),
                    Version = ParseVersion(meta.ProgramVersion),
                    Category = DetermineCategory(meta),
                    OriginInfo = new VarOriginInfo
                    {
                        Filename = Path.GetFileName(varFilePath),
                        Meta = meta
                    },
                    Location = new VarLocation
                    {
                        Directory = Path.GetDirectoryName(varFilePath)
                    },
                    ExtraInfo = new VarExtraInfo
                    {
                        ImportedDate = DateTime.Now,
                        LastUpdated = DateTime.Now
                    }
                };

                return varData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<VamMeta> ExtractMetaJson(string varFilePath)
        {
            if (!File.Exists(varFilePath))
            {
                return null;
            }

            try
            {
                using var archive = ZipFile.OpenRead(varFilePath);
                var metaEntry = archive.GetEntry("meta.json");
                
                if (metaEntry == null)
                {
                    return null;
                }

                using var stream = metaEntry.Open();
                using var reader = new StreamReader(stream);
                
                string jsonContent = await reader.ReadToEndAsync();
                return JsonSerializer.Deserialize<VamMeta>(jsonContent);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public CategoryType DetermineCategory(VamMeta meta)
        {
            if (meta == null || meta.ContentList == null)
            {
                return CategoryType.None;
            }

            // 根据内容列表判断分类
            foreach (var item in meta.ContentList)
            {
                if (item.Contains("scene", StringComparison.OrdinalIgnoreCase))
                {
                    return CategoryType.Scenes;
                }
                
                if (item.Contains("appearance", StringComparison.OrdinalIgnoreCase) || 
                    item.Contains("looks", StringComparison.OrdinalIgnoreCase) ||
                    item.Contains("preset", StringComparison.OrdinalIgnoreCase))
                {
                    return CategoryType.Appearance;
                }
                
                if (item.Contains("plugin", StringComparison.OrdinalIgnoreCase) ||
                    item.Contains("script", StringComparison.OrdinalIgnoreCase) ||
                    item.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                {
                    return CategoryType.Plugin;
                }
            }

            return CategoryType.None;
        }

        private int ParseVersion(string versionString)
        {
            if (string.IsNullOrEmpty(versionString))
            {
                return 0;
            }

            // 尝试解析第一个数字作为版本号
            var parts = versionString.Split('.');
            if (parts.Length > 0 && int.TryParse(parts[0], out int version))
            {
                return version;
            }

            return 0;
        }
    }
} 