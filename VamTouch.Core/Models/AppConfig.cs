using System;

namespace VamTouch.Core.Models
{
    public class AppConfig
    {
        // 管理的var文件所在目录
        public string VarDirectoryPath { get; set; }

        // 上次操作时间
        public DateTime LastAccessed { get; set; }

        // 应用程序版本号
        public string AppVersion { get; set; }
    }
} 