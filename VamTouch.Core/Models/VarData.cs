using System;
using System.Collections.Generic;

namespace VamTouch.Core.Models
{
    // 原始的var信息, 包括原始的文件名和原始的meta.json解析内容
    public class VarOriginInfo
    {
        public string Filename { get; set; }  // 原始的var文件名
        public VamMeta Meta { get; set; }     // 原始的meta.json内容
    }

    // var文件在管理目录中的位置信息
    public class VarLocation
    {
        public string Directory { get; set; } // 文件所在目录
    }

    // 额外信息
    public class VarExtraInfo
    {
        public DateTime ImportedDate { get; set; } // 导入日期
        public DateTime LastUpdated { get; set; }  // 最后更新日期
    }

    public enum CategoryType
    {
        None = 0,      // 无
        Appearance = 1, // 人物外观
        Scenes = 2,    // 场景
        Plugin = 3     // 插件
    }

    // 标准Tag
    public class VarTag
    {
        public string Name { get; set; }
        public CategoryType Category { get; set; }
    }

    // 本项目的核心数据结构，和var文件一一对应
    public class VarData
    {
        public string Id => $"{PackageName}_{Creator}_{OriginInfo?.Filename}".Replace(" ", "_");
        
        public string Creator { get; set; }     // 本项目分析后的包作者
        public string PackageName { get; set; } // 本项目分析后的包名
        public int Version { get; set; }        // 本项目分析后的版本号
        
        public VarOriginInfo OriginInfo { get; set; } // 原始信息
        public VarLocation Location { get; set; }     // var文件的实际位置
        public VarExtraInfo ExtraInfo { get; set; }   // 额外信息
        public List<VarTag> Tags { get; set; } = new List<VarTag>(); // var附带的Tag
        
        public CategoryType Category { get; set; } = CategoryType.None; // 分类
    }
} 