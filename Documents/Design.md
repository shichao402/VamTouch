# 设计文档大纲

## 1. 概述
- 项目背景：开发一个管理.var文件的桌面应用程序，.var文件是包含meta.json的zip压缩文件。
- 功能需求：
  - 首次运行程序需要指定一个目录
  - 读取目录中的所有.var文件，提取并管理文件信息。并保存管理信息.
  - 下一次运行时首先加载之前指定的目录.并刷新文件信息.
- 技术选型：C# 9.0, .NET, xUnit, Avalonia

## 2. 核心模块设计
### 2.1 文件管理模块
- 遍历指定目录，查找所有.var文件
- 提供文件操作接口

### 2.2 文件解析模块
- 解析.var文件，提取meta.json内容
- 数据验证和错误处理

### 2.3 元数据处理模块
- 定义VarData数据结构
- VarData数据缓存和管理

#### VarMeta 数据结构
```csharp
// 原始的var信息, 包括原始的文件名和原始的meta.json解析内容.
public class VarOriginInfo {
    public string Filename { get; set; }  // 原始的var文件名
    public VamMeta Meta { get; set; }  // 原始的meta.json内容
}

// var文件在管理目录中的位置信息
public class VarLocaltion {
    public string Directory {get; set;} // 文件所在目录
}

// 额外信息
public class VarExtraInfo {
}

public enum CategoryType {
    None = 0; // 无
    Apperance = 1; // 人物外观
    Scenes = 2; // 场景
    Plugin = 2; // 插件
}

// 标准Tag
public class VarTag {
}

// 本项目的核心数据结构. 和var文件一一对应.
public class VarData {
    public string Creator { get; set; }  // 本项目分析后的包作者
    public string PackageName { get; set; }  // 本项目分析后的包名
    public int Version { get; set; }  // 本项目分析后的版本号
    public VarOriginInfo originInfo {get; set;} // 原始信息.
    public VarLocation Localtion {get;set;} // var文件的实际位置
    public VarExtraInfo ExtraInfo {get;set;} // 额外信息.
    public VarTag Tags {get;set;} // var附带的Tag
}
```

#### meta.json 文件结构
```json
{
    "licenseType": "CC BY-NC-SA",
    "creatorName": "TRASHINATOR",
    "packageName": "left_right_prophet_cowgirl_sex_dlc",
    "standardReferenceVersionOption": "Latest",
    "scriptReferenceVersionOption": "Exact",
    "description": "",
    "credits": "",
    "instructions": "",
    "promotionalLink": "",
    "programVersion": "1.22.0.12",
    "contentList": [
        "Saves/scene/left right prophet cowgirl sex dlc.jpg",
        "Saves/scene/left right prophet cowgirl sex dlc.json",
        "Custom/Sounds/PussySFX/WetIn19.wav",
        "Custom/Scripts/SoftPhysicsSelectorV2.cs",
        "Custom/Sounds/squishypie.mp3",
        "Custom/Atom/Person/Clothing/Preset_leftrightcowgirl.vap",
        "Custom/Atom/Person/Clothing/Preset_leftrightcowgirl.jpg"
    ],
    "dependencies": {
        "TRASHINATOR.left_right_prophet_boob_job.1": {
            "licenseType": "CC BY-NC-SA",
            "dependencies": {
                "AcidBubbles.Timeline.287": {
                    "licenseType": "CC BY-SA",
                    "dependencies": {}
                },
                "TRASHINATOR.so_young_appearance_look_preset.latest": {
                    "licenseType": "CC BY-NC-SA",
                    "dependencies": {
                        "YameteOuji.YameteOuji_Under_PantyhoseWaist.latest": {
                            "licenseType": "CC BY",
                            "dependencies": {}
                        },
                        "AcidBubbles.Timeline.287": {
                            "licenseType": "CC BY-SA",
                            "dependencies": {}
                        }
                    }
                }
            }
        },
        "AcidBubbles.ImprovedPoV.1": {
            "licenseType": "CC BY-SA",
            "dependencies": {}
        },
        "Redeyes.Yet_Another_Cum_Pack.latest": {
            "licenseType": "CC BY",
            "dependencies": {}
        }
    },
    "customOptions": {
        "preloadMorphs": "false"
    },
    "hadReferenceIssues": "false",
    "referenceIssues": []
}
```

## 3. 界面设计

### 主界面
- 初次进入或者配置文件无法找到时. 显示一个配置界面. 配置界面需要指定一个目录. 这个目录就是本项目要管理的var文件所在目录. 配置将会被保存下来供下次启动使用.
- 存在配置文件时,加载配置文件并进入主界面. 主界面包含一个左侧主要列表区域和右侧功能按钮区域. 以及底部的任务列表区域.

#### 主界面主要列表区域
- 本区域罗列了所有管理的var文件.支持多选. 支持右键菜单. 右键菜单包括删除和打开文件所在位置
- 本区域支持列表显示和缩略图显示切换

#### 主界面右侧功能按钮区域
-本区域提供一些功能按钮.例如"刷新",会更新所有var信息.

#### 主界面下方进度列表区域
-本区域会显示当前正在执行的任务以及他们的进度.例如主列表区域执行删除时. 在这里会显示任务记录.以及实时进度.

## 3. 测试策略
- 单元测试：验证文件解析和VarData数据处理
- 集成测试：验证整个文件管理流程

## 4. 部署与维护
- 无需制作安装包. 但是需要编写构建过程中的配置. 例如拷贝必要文件到构建结果目录.
- 更新机制, 每一次构建都需要维护一个版本号.自增. 这个文件会跟随版本控制一起提交.