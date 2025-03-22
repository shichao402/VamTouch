using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace VamTouch.App.Converters
{
    public class ViewModeIconConverter : IValueConverter
    {
        // 网格视图图标（Material Design - Grid View）
        private const string GridViewIcon = "M3 3V11H11V3H3M3 13V21H11V13H3M13 3V11H21V3H13M13 13V21H21V13H13Z";
        
        // 列表视图图标（Material Design - List View）
        private const string ListViewIcon = "M3 4H21V8H3V4M3 10H21V14H3V10M3 16H21V20H3V16Z";

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isGridView)
            {
                // 如果当前是网格视图，返回列表视图图标，反之亦然
                return isGridView ? ListViewIcon : GridViewIcon;
            }

            return GridViewIcon; // 默认图标
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 