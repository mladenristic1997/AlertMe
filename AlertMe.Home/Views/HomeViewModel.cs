using AlertMe.Domain;
using AlertMe.Domain.Events;
using AlertMe.Timeline.AlertCheckpoint;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AlertMe.Home
{
    public class HomeViewModel : BindableBase
    {
        readonly IEventAggregator EventAggregator;
        readonly ILocalDataStore Store;

        public List<AvailableDropdownPlan> AvailablePlans { get; set; }

        AvailableDropdownPlan selectedPlan;
        public AvailableDropdownPlan SelectedPlan
        {
            get => selectedPlan;
            set => SetProperty(ref selectedPlan, value);
        }

        public HomeViewModel(IEventAggregator ea, ILocalDataStore store)
        {
            EventAggregator = ea;
            AvailablePlans = new List<AvailableDropdownPlan>();
            Store = store;
            LoadStoredPlans();
            EventAggregator.GetEvent<LocalStoreChanged>().Subscribe(OnLocalStoreChanged);
        }

        void OnLocalStoreChanged()
        {
            foreach (var plan in AvailablePlans)
            {
                var vm = plan.Plan.DataContext as PlanViewModel;
                vm.ResetTimeline();
            }
            LoadStoredPlans();
        }
        
        void LoadStoredPlans()
        {
            AvailablePlans.Clear();
            var c = Store.GetObject<StoredAlertPlans>();
            if (c != null)
            {
                foreach (var plan in c.AlertPlans)
                {
                    var alertPlan = Store.GetObject<AlertPlan>(plan);
                    var timelineAlerts = new ObservableCollection<Timeline.Alert>();
                    var alertCheckpoints = new ObservableCollection<AlertCheckpointViewModel>();
                    var alertTimes = new List<int>();
                    foreach (var alert in alertPlan.Alerts)
                    {
                        var ta = new Timeline.Alert
                        {
                            Id = alert.Id,
                            Message = alert.Message,
                            TotalSeconds = CalculateInSeconds(alert.Hours, alert.Minutes, alert.Seconds)
                        };
                        timelineAlerts.Add(ta);
                    }
                    var seconds = 0;
                    foreach (var a in timelineAlerts)
                    {
                        seconds += a.TotalSeconds;
                        alertTimes.Add(seconds);
                        var ac = new AlertCheckpointViewModel();
                        ac.Id = a.Id;
                        ac.Message = a.Message;
                        ac.AlertAt = ParseTime(seconds);
                        ac.TotalSeconds = a.TotalSeconds;
                        ac.IsVisible = ac.TotalSeconds != 0;
                        ac.Margin = new Thickness(CalculateMargin(seconds, timelineAlerts.Sum(x => x.TotalSeconds)) + 14, 0, 14, 0);
                        alertCheckpoints.Add(ac);
                    }
                    var vm = new PlanViewModel(EventAggregator) { Id = alertPlan.Id, AlertCheckpoints = alertCheckpoints, AlertTimes = alertTimes, ProgressMargin = new Thickness(14, 0, 14, 0), PlanDuration = timelineAlerts.Sum(x => x.TotalSeconds) };
                    AvailablePlans.Add(new AvailableDropdownPlan { Name = alertPlan.Name, Plan = new PlanView { DataContext = vm } });
                }
            }
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
                "00"
                :
                count.ToString();

            double CalculateMargin(int time, int planDuration) => Math.Round(750.0 * time / planDuration, 2);

            int CalculateInSeconds(int h, int m, int s) => h * 60 * 60 + m * 60 + s;
    }

    public class AvailableDropdownPlan : BindableBase
    {
        string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        Control plan;
        public Control Plan
        {
            get => plan;
            set => SetProperty(ref plan, value);
        }
    }
}
