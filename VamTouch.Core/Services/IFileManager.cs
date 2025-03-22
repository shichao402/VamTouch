using System.Collections.Generic;
using System.Threading.Tasks;
using VamTouch.Core.Models;

namespace VamTouch.Core.Services
{
    public interface IFileManager
    {
        /// <summary>
        /// 设置要管理的VAR文件目录
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <returns>是否设置成功</returns>
        Task<bool> SetVarDirectory(string directoryPath);
        
        /// <summary>
        /// 获取当前管理的VAR文件目录
        /// </summary>
        /// <returns>目录路径</returns>
        string GetVarDirectory();
        
        /// <summary>
        /// 扫描指定目录中的所有VAR文件
        /// </summary>
        /// <returns>找到的VAR文件路径列表</returns>
        Task<List<string>> ScanVarFiles();
        
        /// <summary>
        /// 打开VAR文件所在位置
        /// </summary>
        /// <param name="varData">VAR数据</param>
        /// <returns>是否成功打开</returns>
        Task<bool> OpenVarFileLocation(VarData varData);
        
        /// <summary>
        /// 删除VAR文件
        /// </summary>
        /// <param name="varData">要删除的VAR文件数据</param>
        /// <returns>是否删除成功</returns>
        Task<bool> DeleteVarFile(VarData varData);
        
        /// <summary>
        /// 打开文件所在位置
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>是否成功打开</returns>
        Task<bool> OpenFileLocation(string filePath);
        
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath">要删除的文件路径</param>
        /// <returns>是否删除成功</returns>
        Task<bool> DeleteVarFile(string filePath);
    }
} 