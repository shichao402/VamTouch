using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VamTouch.Core.Models;
using VamTouch.Core.Services;

namespace VamTouch.App.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly IFileManager _fileManager;
        private readonly IVarDataManager _varDataManager;
        private readonly IConfigManager _configManager;

        [ObservableProperty]
        private bool _isConfigured;

        [ObservableProperty]
        private string _directoryPath = string.Empty;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private string _statusMessage = "准备就绪";

        [ObservableProperty]
        private ObservableCollection<VarDataViewModel> _varItems = new();

        [ObservableProperty]
        private bool _isGridView = true;

        [ObservableProperty]
        private VarDataViewModel? _selectedVarItem;

        [ObservableProperty]
        private ObservableCollection<TaskProgressViewModel> _tasks = new();

        public MainViewModel(IFileManager fileManager, IVarDataManager varDataManager, IConfigManager configManager)
        {
            _fileManager = fileManager;
            _varDataManager = varDataManager;
            _configManager = configManager;

            // 异步初始化
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            IsLoading = true;
            StatusMessage = "正在初始化...";

            try
            {
                // 检查是否已配置
                IsConfigured = _configManager.ConfigExists();

                if (IsConfigured)
                {
                    // 加载配置
                    var config = await _configManager.LoadConfig();
                    DirectoryPath = config.VarDirectoryPath;

                    // 加载VAR数据状态
                    await _varDataManager.LoadVarDataState();

                    // 刷新VAR数据
                    await RefreshVarData();
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"初始化失败: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task SelectDirectory()
        {
            var topLevel = GetTopLevel();
            if (topLevel == null)
            {
                StatusMessage = "无法获取窗口句柄";
                return;
            }

            // 使用新的StorageProvider API
            var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "选择VAR文件目录",
                AllowMultiple = false
            });

            if (folders.Count > 0)
            {
                var folder = folders[0];
                DirectoryPath = folder.Path.LocalPath;
                await SetDirectory(DirectoryPath);
            }
        }

        private TopLevel? GetTopLevel()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                return desktop.MainWindow;
            }
            return null;
        }

        [RelayCommand]
        private async Task RefreshVarData()
        {
            if (string.IsNullOrEmpty(DirectoryPath))
            {
                return;
            }

            IsLoading = true;
            StatusMessage = "正在刷新VAR文件...";

            try
            {
                // 创建任务
                var task = new TaskProgressViewModel
                {
                    Name = "刷新VAR文件",
                    IsIndeterminate = true,
                    Status = "正在扫描..."
                };
                Tasks.Add(task);

                // 刷新数据
                bool success = await _varDataManager.RefreshVarData();
                if (success)
                {
                    // 更新UI
                    UpdateVarItemsList();
                    StatusMessage = $"已刷新 {VarItems.Count} 个VAR文件";
                    task.Status = $"已完成，找到 {VarItems.Count} 个文件";
                }
                else
                {
                    StatusMessage = "刷新失败";
                    task.Status = "刷新失败";
                }

                task.IsCompleted = true;
                task.Progress = 100;
                task.IsIndeterminate = false;
            }
            catch (Exception ex)
            {
                StatusMessage = $"刷新失败: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SetDirectory(string directoryPath)
        {
            IsLoading = true;
            StatusMessage = "正在设置目录...";

            try
            {
                // 设置目录
                bool success = await _fileManager.SetVarDirectory(directoryPath);
                if (!success)
                {
                    StatusMessage = "设置目录失败";
                    return;
                }

                IsConfigured = true;
                DirectoryPath = directoryPath;

                // 创建任务
                var task = new TaskProgressViewModel
                {
                    Name = "扫描VAR文件",
                    IsIndeterminate = true,
                    Status = "正在扫描..."
                };
                Tasks.Add(task);

                // 扫描VAR文件
                var files = await _fileManager.ScanVarFiles();
                task.Status = $"找到 {files.Count} 个文件，正在解析...";

                // 加载VAR数据
                var varDataList = await _varDataManager.LoadVarData(files);
                
                // 更新UI
                UpdateVarItemsList();
                
                StatusMessage = $"已加载 {VarItems.Count} 个VAR文件";
                task.Status = $"已完成，已加载 {VarItems.Count} 个文件";
                task.IsCompleted = true;
                task.Progress = 100;
                task.IsIndeterminate = false;
            }
            catch (Exception ex)
            {
                StatusMessage = $"设置目录失败: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void UpdateVarItemsList()
        {
            VarItems.Clear();

            var allVarData = _varDataManager.GetAllVarData();
            foreach (var varData in allVarData)
            {
                VarItems.Add(new VarDataViewModel(varData));
            }
        }

        [RelayCommand]
        private void ToggleViewMode()
        {
            IsGridView = !IsGridView;
        }

        [RelayCommand]
        private async Task OpenFileLocation(object? parameter = null)
        {
            // 使用参数或选中项
            VarDataViewModel? target = parameter as VarDataViewModel ?? SelectedVarItem;
            if (target != null)
            {
                try
                {
                    await _fileManager.OpenFileLocation(target.FilePath);
                    StatusMessage = $"已打开文件位置: {target.Filename}";
                }
                catch (Exception ex)
                {
                    StatusMessage = $"打开文件位置失败: {ex.Message}";
                }
            }
        }

        [RelayCommand]
        private async Task DeleteVarFile(object? parameter = null)
        {
            // 使用参数或选中项
            VarDataViewModel? target = parameter as VarDataViewModel ?? SelectedVarItem;
            if (target != null)
            {
                IsLoading = true;
                StatusMessage = $"正在删除文件: {target.Filename}";

                try
                {
                    bool success = await _fileManager.DeleteVarFile(target.FilePath);
                    if (success)
                    {
                        // 从列表中移除
                        VarItems.Remove(target);
                        StatusMessage = $"已删除文件: {target.Filename}";
                        
                        // 更新数据管理器
                        await _varDataManager.RemoveVarData(target.Id);
                    }
                    else
                    {
                        StatusMessage = $"删除文件失败: {target.Filename}";
                    }
                }
                catch (Exception ex)
                {
                    StatusMessage = $"删除文件失败: {ex.Message}";
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }
    }
} 