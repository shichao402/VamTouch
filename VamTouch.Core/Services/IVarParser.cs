using System.Threading.Tasks;
using VamTouch.Core.Models;

namespace VamTouch.Core.Services
{
    public interface IVarParser
    {
        /// <summary>
        /// 解析VAR文件并提取元数据
        /// </summary>
        /// <param name="varFilePath">VAR文件路径</param>
        /// <returns>解析后的VarData对象</returns>
        Task<VarData> ParseVarFile(string varFilePath);
        
        /// <summary>
        /// 从VAR文件中提取meta.json内容
        /// </summary>
        /// <param name="varFilePath">VAR文件路径</param>
        /// <returns>解析后的meta.json内容</returns>
        Task<VamMeta> ExtractMetaJson(string varFilePath);
        
        /// <summary>
        /// 根据meta.json内容分析VAR文件的分类
        /// </summary>
        /// <param name="meta">meta.json内容</param>
        /// <returns>VAR文件分类</returns>
        CategoryType DetermineCategory(VamMeta meta);
    }
} 