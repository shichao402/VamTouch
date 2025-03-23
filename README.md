# VamTouch

VamTouch是一个管理.var文件的桌面应用程序，.var文件是包含meta.json的zip压缩文件。

## 功能特性

- 读取指定目录中的所有.var文件
- 提取并管理文件信息
- 保存管理信息
- 支持列表视图和网格视图
- 支持打开文件位置和删除文件

## 技术栈

- C# 9.0
- .NET
- Avalonia UI
- xUnit测试框架

## 用户安装与运行

### 方式一：直接下载可执行文件

1. 从[发布页面](releases)下载最新版本的安装包
2. 对于Windows用户：
   - 下载`VamTouch-win-x64.zip`并解压
   - 双击运行`VamTouch.App.exe`
3. 对于macOS用户：
   - 下载`VamTouch-osx-x64.zip`并解压
   - 双击运行`VamTouch.App`
4. 对于Linux用户：
   - 下载`VamTouch-linux-x64.zip`并解压
   - 赋予执行权限：`chmod +x VamTouch.App`
   - 运行：`./VamTouch.App`

### 方式二：从源码编译并发布

1. 安装最新的[.NET SDK](https://dotnet.microsoft.com/download)
2. 克隆项目仓库
3. 在项目根目录运行以下命令发布应用程序：
   ```
   dotnet publish VamTouch.App -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
   ```
   - 将`win-x64`替换为您的目标平台（如`osx-x64`或`linux-x64`）
4. 发布的文件位于`VamTouch.App/bin/Release/net9.0/{平台}/publish/`目录下
5. 运行可执行文件（Windows下为`.exe`文件，macOS和Linux下无扩展名）

## 开发环境设置

1. 安装最新的[.NET SDK](https://dotnet.microsoft.com/download)
2. 克隆项目仓库
3. 在项目根目录运行`dotnet restore`安装依赖
4. 运行`dotnet build`编译项目
5. 运行`dotnet run --project VamTouch.App`启动应用程序（仅用于开发测试）

## 项目结构

- **VamTouch.App**: UI界面项目
- **VamTouch.Core**: 核心业务逻辑
- **VamTouch.Tests**: 单元测试

## 设计文档

设计文档位于[Documents/Design.md](Documents/Design.md)。

## 许可证

[MIT许可证](LICENSE) 