﻿using AlertMe.Plans.Commands;
using AlertMe.Plans.Events;
using AlertMe.Domain;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
//using AlertMe.Timeline;
using Alert = AlertMe.Domain.Alert;

namespace AlertMe.Plans
{
    public class AlertPlanViewModel : BindableBase
    {
        readonly IEventAggregator EventAggregator;

        public DelegateCommand AddNewAlert { get; set; }
        public DelegateCommand Save { get; set; }
        public DelegateCommand Delete { get; set; }

        //public ObservableCollection<Timeline.Alert> TimelineAlerts { get; set; }
        
        public string Id { get; set; }

        string configName;
        public string ConfigName
        {
            get => configName;
            set
            {
                SetProperty(ref configName, value);
            }
        }

        public ObservableCollection<Control> Alerts { get; set; }

        public AlertPlanViewModel(IEventAggregator ea)
        {
            EventAggregator = ea;
            AddNewAlert = new DelegateCommand(OnAddNewAlert);
            Save = new DelegateCommand(OnSave);
            Delete = new DelegateCommand(OnDelete);
            Alerts = new ObservableCollection<Control>();
            //TimelineAlerts = new ObservableCollection<Timeline.Alert>();
            EventAggregator.GetEvent<AlertChanged>().Subscribe(OnAlertChanged);
            EventAggregator.GetEvent<RemoveAlert>().Subscribe(OnRemoveAlert);
        }

        void OnAlertChanged(AlertChangedArgs args)
        {
            foreach (var alert in Alerts)
            {
                var vm = alert.DataContext as AlertViewModel;
                if (vm.Id == args.Alert.Id)
                { 
                    //Left to implement timeline bar
                }
            }
        }

        void OnAddNewAlert()
        {
            var id = IdProvider.GetId();
            Alerts.Add(new AlertView { DataContext = new AlertViewModel(EventAggregator) { Id = id, Alert = new Alert { Id = id } } });
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
        }

        void OnSave()
        {
            var config = new AlertPlan { Id = Id, Name = ConfigName, Alerts = GetAlertObjects() };
            EventAggregator.GetEvent<SaveAlertPlan>().Publish(config);
        }

            List<Alert> GetAlertObjects()
            {
                var list = new List<Alert>();
                foreach (var alert in Alerts)
                {
                    var vm = alert.DataContext as AlertViewModel;
                    list.Add(vm.Alert);
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
    }
}
