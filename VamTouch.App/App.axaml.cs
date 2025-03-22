using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using System;
using VamTouch.App.ViewModels;
using VamTouch.App.Views;
using VamTouch.Core.Services;

namespace VamTouch.App;

public partial class App : Application
{
    private IServiceProvider? _serviceProvider;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // 配置依赖注入
        var services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // 注册核心服务
        services.AddSingleton<IConfigManager, ConfigManager>();
        services.AddSingleton<IFileManager, FileManager>();
        services.AddSingleton<IVarParser, VarParser>();
        services.AddSingleton<IVarDataManager, VarDataManager>();

        // 注册视图模型
        services.AddSingleton<MainViewModel>();
    }
}