using System.Threading.Tasks;
using VamTouch.Core.Models;

namespace VamTouch.Core.Services
{
    public interface IConfigManager
    {
        /// <summary>
        /// 加载应用程序配置
        /// </summary>
        /// <returns>应用程序配置</returns>
        Task<AppConfig> LoadConfig();
        
        /// <summary>
        /// 保存应用程序配置
        /// </summary>
        /// <param name="config">应用程序配置</param>
        /// <returns>是否保存成功</returns>
        Task<bool> SaveConfig(AppConfig config);
        
        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        /// <returns>配置文件路径</returns>
        string GetConfigFilePath();
        
        /// <summary>
        /// 检查配置是否存在
        /// </summary>
        /// <returns>配置文件是否存在</returns>
        bool ConfigExists();
    }
} 