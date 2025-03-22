using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace VamTouch.App.ViewModels
{
    public partial class TaskProgressViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private string _status = string.Empty;

        [ObservableProperty]
        private double _progress;

        [ObservableProperty]
        private bool _isIndeterminate;

        [ObservableProperty]
        private bool _isCompleted;

        [ObservableProperty]
        private DateTime _startTime = DateTime.Now;

        [ObservableProperty]
        private DateTime? _endTime;
    }
} 