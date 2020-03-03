using AlertMe.Config.Commands;
using AlertMe.Config.Events;
using AlertMe.Domain;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace AlertMe.Config
{
    public class AlertConfigViewModel : BindableBase
    {
        readonly IEventAggregator EventAggregator;

        public DelegateCommand AddNewAlert { get; set; }
        public DelegateCommand Save { get; set; }
        public DelegateCommand Delete { get; set; }
        
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

        public AlertConfigViewModel(IEventAggregator ea)
        {
            EventAggregator = ea;
            AddNewAlert = new DelegateCommand(OnAddNewAlert);
            Save = new DelegateCommand(OnSave);
            Delete = new DelegateCommand(OnDelete);
            Alerts = new ObservableCollection<Control>();
            EventAggregator.GetEvent<AlertChanged>().Subscribe(OnConfigChanged);
            EventAggregator.GetEvent<RemoveAlert>().Subscribe(OnRemoveAlert);
        }

        void OnConfigChanged(AlertChangedArgs args)
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
            var config = new AlertConfig { Id = Id, Name = ConfigName, Alerts = GetAlertObjects() };
            EventAggregator.GetEvent<SaveConfig>().Publish(config);
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
            EventAggregator.GetEvent<AlertChanged>().Unsubscribe(OnConfigChanged);
            EventAggregator.GetEvent<RemoveAlert>().Unsubscribe(OnRemoveAlert);
            EventAggregator.GetEvent<DeleteAlertConfig>().Publish(new DeleteAlertConfigArgs { Id = Id });
        }
    }
}
