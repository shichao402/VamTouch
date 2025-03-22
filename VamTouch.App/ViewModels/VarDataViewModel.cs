using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using VamTouch.Core.Models;

namespace VamTouch.App.ViewModels
{
    public partial class VarDataViewModel : ViewModelBase
    {
        public VarData VarData { get; }
        
        public string Id => VarData.Id;
        
        public string FilePath => Path.Combine(Directory, Filename);

        [ObservableProperty]
        private string _filename;

        [ObservableProperty]
        private string _creator;

        [ObservableProperty]
        private string _packageName;

        [ObservableProperty]
        private string _version;

        [ObservableProperty]
        private string _category;

        [ObservableProperty]
        private DateTime _importDate;

        [ObservableProperty]
        private string _directory;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private string _license;

        public VarDataViewModel(VarData varData)
        {
            VarData = varData;

            // 填充属性
            Filename = varData.OriginInfo?.Filename ?? "未知文件名";
            Creator = varData.Creator ?? "未知创建者";
            PackageName = varData.PackageName ?? "未知包名";
            Version = varData.Version.ToString();
            Category = GetCategoryName(varData.Category);
            ImportDate = varData.ExtraInfo?.ImportedDate ?? DateTime.Now;
            Directory = varData.Location?.Directory ?? "未知目录";
            Description = varData.OriginInfo?.Meta?.Description ?? "无描述";
            License = varData.OriginInfo?.Meta?.LicenseType ?? "未知许可证";
        }

        private string GetCategoryName(CategoryType category)
        {
            return category switch
            {
                CategoryType.None => "未分类",
                CategoryType.Appearance => "人物外观",
                CategoryType.Scenes => "场景",
                CategoryType.Plugin => "插件",
                _ => "未知"
            };
        }
    }
} 