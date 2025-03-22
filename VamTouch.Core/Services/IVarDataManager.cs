using System.Collections.Generic;
using System.Threading.Tasks;
using VamTouch.Core.Models;

namespace VamTouch.Core.Services
{
    public interface IVarDataManager
    {
        /// <summary>
        /// 加载并解析所有VAR文件
        /// </summary>
        /// <param name="varFilePaths">VAR文件路径列表</param>
        /// <returns>解析后的VAR数据列表</returns>
        Task<List<VarData>> LoadVarData(List<string> varFilePaths);
        
        /// <summary>
        /// 刷新VAR数据
        /// </summary>
        /// <returns>是否刷新成功</returns>
        Task<bool> RefreshVarData();
        
        /// <summary>
        /// 获取所有VAR数据
        /// </summary>
        /// <returns>VAR数据列表</returns>
        List<VarData> GetAllVarData();
        
        /// <summary>
        /// 根据分类获取VAR数据
        /// </summary>
        /// <param name="category">分类</param>
        /// <returns>该分类下的VAR数据列表</returns>
        List<VarData> GetVarDataByCategory(CategoryType category);
        
        /// <summary>
        /// 根据创建者获取VAR数据
        /// </summary>
        /// <param name="creator">创建者</param>
        /// <returns>该创建者的VAR数据列表</returns>
        List<VarData> GetVarDataByCreator(string creator);
        
        /// <summary>
        /// 保存VAR数据管理状态
        /// </summary>
        /// <returns>是否保存成功</returns>
        Task<bool> SaveVarDataState();
        
        /// <summary>
        /// 加载VAR数据管理状态
        /// </summary>
        /// <returns>是否加载成功</returns>
        Task<bool> LoadVarDataState();
        
        /// <summary>
        /// 移除VAR数据
        /// </summary>
        /// <param name="id">要移除的VAR数据ID</param>
        /// <returns>是否移除成功</returns>
        Task<bool> RemoveVarData(string id);
    }
} 