using AlertMe.Timeline.AlertCheckpoint;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

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

        public ObservableCollection<AlertCheckpointViewModel> AlertCheckpoints { get; set; }

        public TimelineViewModel()
        {
            Alerts = new ObservableCollection<Alert>();
            AlertCheckpoints = new ObservableCollection<AlertCheckpointViewModel>();
        }

        public void UpdateView()
        {
            AlertCheckpoints = null;
            foreach (var a in Alerts)
                a.Update = null;
            var list = new ObservableCollection<AlertCheckpointViewModel>();
            int seconds = 0;
            foreach (var a in Alerts)
            {
                seconds += a.TotalSeconds;
                var vm = new AlertCheckpointViewModel();
                vm.Id = a.Id;
                vm.Message = a.Message;
                vm.AlertAt = ParseTime(seconds);
                vm.LeftMargin = CalculateMargin(a.TotalSeconds);
                list.Add(vm);
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
                return $"{GetTime(hours)}:{GetTime(minutes)}:{GetTime(seconds)}";
            }

        string GetTime(int count) => count.ToString() == string.Empty ?
            "0"
            :
            count.ToString();

            double CalculateMargin(int time) => Math.Round((500.0 * time / PlanDuration) - 14, 2);
    }
}
