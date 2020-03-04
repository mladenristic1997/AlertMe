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

        public AlertPlanViewModel(IEventAggregator ea)
        {
            EventAggregator = ea;
            AddNewAlert = new DelegateCommand(OnAddNewAlert);
            Save = new DelegateCommand(OnSave);
            Delete = new DelegateCommand(OnDelete);
            Alerts = new ObservableCollection<Control>();
            TimelineAlerts = new ObservableCollection<Timeline.Alert>();
            EventAggregator.GetEvent<AlertChanged>().Subscribe(OnAlertChanged);
            EventAggregator.GetEvent<RemoveAlert>().Subscribe(OnRemoveAlert);
        }

        void OnAlertChanged(AlertChangedArgs args)
        {
            UpdatePlanDuration();
            foreach (var a in TimelineAlerts)
                if (a.Id == args.Id)
                {
                    a.Message = args.Message;
                    a.TotalSeconds = CalculateInSeconds(args.Hours, args.Minutes, args.Seconds);
                }
        }

        int CalculateInSeconds(int h, int m, int s) => h * 60 * 60 + m * 60 + s;

        void OnAddNewAlert()
        {
            var id = IdProvider.GetId();
            Alerts.Add(new AlertView { DataContext = new AlertViewModel(EventAggregator) { Id = id } });
            TimelineAlerts.Add(new Timeline.Alert { Id = id });
            UpdatePlanDuration();
        }

        void OnRemoveAlert(RemoveAlertArgs args)
        {
            foreach (var alert in Alerts)
            {
                var vm = alert.DataContext as AlertViewModel;
                if (vm.Id == args.Id)
                {
                    Alerts.Remove(alert);
                    return;
                }
            }
            foreach (var alert in TimelineAlerts)
            {
                if (alert.Id == args.Id)
                {
                    TimelineAlerts.Remove(alert);
                    return;
                }
            }
            UpdatePlanDuration();
        }

        void OnSave()
        {
            var config = new AlertPlan { Id = Id, Name = PlanName, Alerts = GetAlertObjects() };
            EventAggregator.GetEvent<SaveAlertPlan>().Publish(config);
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

        public void UpdatePlanDuration()
        {
            var vms = Alerts.Select(x => x.DataContext as AlertViewModel).ToDictionary(x => x.Id, x => x);
            PlanDuration = vms.Sum(x => CalculateInSeconds(x.Value.Hours, x.Value.Minutes, x.Value.Seconds));
        }
    }
}
