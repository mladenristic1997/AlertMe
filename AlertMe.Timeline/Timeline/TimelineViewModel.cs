
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace AlertMe.Timeline
{
    public class TimelineViewModel : BindableBase
    {
        int planDuration;
        public int PlanDuration
        {
            get => planDuration;
            set
            {
                SetProperty(ref planDuration, value);
                UpdateView();
            }
        }

        double controlWidth;
        public double ControlWidth
        {
            get => controlWidth;
            set
            {
                SetProperty(ref controlWidth, value);
                UpdateView();
            }
        }

        public ObservableCollection<Alert> Alerts { get; set; }

        public ObservableCollection<AlertCheckpoint.AlertCheckpoint> AlertCheckpoints { get; set; }

        public TimelineViewModel()
        {
            Alerts = new ObservableCollection<Alert>();
            AlertCheckpoints = new ObservableCollection<AlertCheckpoint.AlertCheckpoint>();
        }

        public void UpdateView()
        {
            var list = new ObservableCollection<AlertCheckpoint.AlertCheckpoint>();
            int seconds = 0;
            foreach (var a in Alerts)
            {
                seconds += a.TotalSeconds;
                var vm = new AlertCheckpoint.AlertCheckpointViewModel();
                vm.Id = a.Id;
                vm.Message = a.Message;
                vm.AlertAt = ParseTime(seconds);
                vm.Margin = CalculateMargin(a.TotalSeconds);
            }
            AlertCheckpoints = list;
        }

            string ParseTime(int totalSeconds)
            {
                var hours = totalSeconds / 3600;
                totalSeconds = totalSeconds % 3600;
                var minutes = totalSeconds / 60;
                totalSeconds = totalSeconds % 60;
                var seconds = totalSeconds;
                return $"{hours.ToString("##")}:{minutes.ToString("##")}:{seconds.ToString("##")}";
            }

            Thickness CalculateMargin(int time) => new Thickness((ControlWidth * time / PlanDuration) - 14, 0, 0, 0);
    }
}
