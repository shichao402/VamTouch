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

## 开发环境设置

1. 安装最新的[.NET SDK](https://dotnet.microsoft.com/download)
2. 克隆项目仓库
3. 在项目根目录运行`dotnet restore`安装依赖
4. 运行`dotnet build`编译项目
5. 运行`dotnet run --project VamTouch.App`启动应用程序

## 项目结构

- **VamTouch.App**: UI界面项目
- **VamTouch.Core**: 核心业务逻辑
- **VamTouch.Tests**: 单元测试

## 设计文档

设计文档位于[Documents/Design.md](Documents/Design.md)。

## 许可证

[MIT许可证](LICENSE) 