using AlertMe.Plans.Commands;
using AlertMe.Plans.Events;
using AlertMe.Domain;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Alert = AlertMe.Domain.Alert;
using System.Linq;
using AlertMe.Timeline.AlertCheckpoint;
using System;

namespace AlertMe.Plans
{
    public class AlertPlanViewModel : BindableBase
    {
        readonly IEventAggregator EventAggregator;

        public DelegateCommand AddNewAlert { get; set; }
        public DelegateCommand Save { get; set; }
        public DelegateCommand Delete { get; set; }
        
        public string Id { get; set; }

        string planName;
        public string PlanName
        {
            get => planName;
            set
            {
                SetProperty(ref planName, value);
            }
        }

        int planDuration;
        public int PlanDuration
        {
            get => planDuration;
            set => SetProperty(ref planDuration, value);
        }

        public ObservableCollection<Control> Alerts { get; set; }
        public ObservableCollection<Timeline.Alert> TimelineAlerts { get; set; }
        public ObservableCollection<AlertCheckpointViewModel> AlertCheckpoints { get; set; }

        public AlertPlanViewModel(IEventAggregator ea)
        {
            EventAggregator = ea;
            AddNewAlert = new DelegateCommand(OnAddNewAlert);
            Save = new DelegateCommand(OnSave);
            Delete = new DelegateCommand(OnDelete);
            Alerts = new ObservableCollection<Control>();
            TimelineAlerts = new ObservableCollection<Timeline.Alert>();
            AlertCheckpoints = new ObservableCollection<AlertCheckpointViewModel>();
            EventAggregator.GetEvent<AlertChanged>().Subscribe(OnAlertChanged);
            EventAggregator.GetEvent<RemoveAlert>().Subscribe(OnRemoveAlert);
        }

        void OnAlertChanged(AlertChangedArgs args)
        {
            if (!AlertCheckpoints.Select(x => x.Id).Where(x => x == args.Id).Any())
                return;
            PlanDuration = CalculateNewPlanDuration(args);
            foreach (var a in TimelineAlerts)
            {
                if (a.Id == args.Id)
                {
                    a.Message = args.Message;
                    a.TotalSeconds = CalculateInSeconds(args.Hours, args.Minutes, args.Seconds);
                    break;
                }
            }
            int seconds = 0;
            foreach (var a in AlertCheckpoints)
            {
                if (a.Id == args.Id)
                {
                    a.Message = args.Message;
                    a.TotalSeconds = CalculateInSeconds(args.Hours, args.Minutes, args.Seconds);
                    a.AlertAt = ParseTime(seconds + a.TotalSeconds);
                    a.Margin = new Thickness(CalculateMargin(seconds + a.TotalSeconds, PlanDuration) + 14, 0, 14, 0);
                    a.IsVisible = a.TotalSeconds != 0;
                    break;
                }
                seconds += a.TotalSeconds;
            }
            UpdateAlertCheckpoints();
            EventAggregator.GetEvent<FinishedViewModelUpdate>().Publish();
        }

        int CalculateNewPlanDuration(AlertChangedArgs args)
        {
            var duration = 0;
            foreach (var a in TimelineAlerts)
            {
                if (a.Id == args.Id)
                {
                    duration += CalculateInSeconds(args.Hours, args.Minutes, args.Seconds);
                    continue;
                }
                duration += a.TotalSeconds;
            }
            return duration;
        }

        int CalculateNewPlanDuration()
        {
            var duration = 0;
            foreach (var a in TimelineAlerts)
                duration += a.TotalSeconds;
            return duration;
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

        void OnAddNewAlert()
        {
            var id = IdProvider.GetId();
            Alerts.Add(new AlertView { DataContext = new AlertViewModel(EventAggregator) { Id = id, PlanId = Id } });
            TimelineAlerts.Add(new Timeline.Alert { Id = id });
            AlertCheckpoints.Add(new AlertCheckpointViewModel { Id = id, IsVisible = false });
        }

        void OnRemoveAlert(RemoveAlertArgs args)
        {
            if (!AlertCheckpoints.Select(x => x.Id).Where(x => x == args.Id).Any())
                return;
            foreach (var alert in Alerts)
            {
                var vm = alert.DataContext as AlertViewModel;
                if (vm.Id == args.Id)
                {
                    Alerts.Remove(alert);
                    break;
                }
            }
            foreach (var alert in TimelineAlerts)
            {
                if (alert.Id == args.Id)
                {
                    TimelineAlerts.Remove(alert);
                    break;
                }
            }
            foreach (var alert in AlertCheckpoints)
            {
                if (alert.Id == args.Id)
                {
                    AlertCheckpoints.Remove(alert);
                    break;
                }
            }
            PlanDuration = CalculateNewPlanDuration();
            UpdateAlertCheckpoints();
            EventAggregator.GetEvent<FinishedViewModelUpdate>().Publish();
        }

        void UpdateAlertCheckpoints()
        {
            int seconds = 0;
            foreach (var a in AlertCheckpoints)
            {
                a.AlertAt = ParseTime(seconds + a.TotalSeconds);
                a.Margin = new Thickness(CalculateMargin(seconds + a.TotalSeconds, PlanDuration), 0, 0, 0);
                seconds += a.TotalSeconds;
            }
        }

        void OnSave()
        {
            var plan = new AlertPlan { Id = Id, Name = PlanName, Alerts = GetAlertObjects() };
            EventAggregator.GetEvent<SaveAlertPlan>().Publish(plan);
        }

            List<Alert> GetAlertObjects()
            {
                var list = new List<Alert>();
                foreach (var alert in Alerts)
                {
                    var vm = alert.DataContext as AlertViewModel;
                    list.Add(new Alert { 
                        Id = vm.Id,
                        Hours = vm.Hours,
                        Minutes = vm.Minutes,
                        Seconds = vm.Seconds,
                        Message = vm.Message
                    });
                }
                return list;
            }

        void OnDelete()
        {
            if (MessageBox.Show("Are you sure?", "Delete", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel) != MessageBoxResult.Yes) 
                return;
            EventAggregator.GetEvent<AlertChanged>().Unsubscribe(OnAlertChanged);
            EventAggregator.GetEvent<RemoveAlert>().Unsubscribe(OnRemoveAlert);
            EventAggregator.GetEvent<DeleteAlertPlan>().Publish(new DeleteAlertPlanArgs { Id = Id });
        }

        #region timeline behavior

        #endregion
    }
}
