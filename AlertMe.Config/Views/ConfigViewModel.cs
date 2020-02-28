using AlertMe.Config.Commands;
using AlertMe.Config.Events;
using AlertMe.Domain;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AlertMe.Config
{
    public class ConfigViewModel : BindableBase
    {
        readonly IEventAggregator EventAggregator;
        readonly ILocalDataStore Store;

        string newConfigName;
        public string NewConfigName
        {
            get => newConfigName;
            set => SetProperty(ref newConfigName, value);
        }

        public DelegateCommand AddNewConfigCommand { get; set; }

        public ObservableCollection<DropdownConfig> Configs { get; set; }

        DropdownConfig selectedConfig;
        public DropdownConfig SelectedConfig
        {
            get => selectedConfig;
            set => SetProperty(ref selectedConfig, value);
        }

        public ConfigViewModel(IEventAggregator ea, ILocalDataStore store)
        {
            EventAggregator = ea;
            Store = store;
            LoadStoredConfigs();
            Configs = new ObservableCollection<DropdownConfig>();
            AddNewConfigCommand = new DelegateCommand(OnAddNewConfig);
            EventAggregator.GetEvent<SaveConfig>().Subscribe(OnSaveConfig);
            EventAggregator.GetEvent<DeleteAlertConfig>().Subscribe(OnDeleteConfig);
        }

        void LoadStoredConfigs()
        {
            var c = Store.GetObject<StoredAlertConfigs>();
            if (c != null)
            {
                foreach (var cfg in c.AlertConfigs)
                {
                    var cf = Store.GetObject<AlertConfig>(cfg);
                    var alertsList = new ObservableCollection<Control>();
                    foreach (var alert in cf.Alerts)
                    {
                        var avm = new AlertViewModel(EventAggregator) { Id = alert.Id, Alert = alert };
                        alertsList.Add(new AlertView { DataContext = avm });
                    }
                    var vm = new AlertConfigViewModel(EventAggregator) { ConfigName = cf.Name, Id = cf.Id, Alerts = alertsList };
                    Configs.Add(new DropdownConfig { Name = cf.Name, Config = new AlertConfigView { DataContext = vm } });
                }
            }
        }

        void OnAddNewConfig()
        {
            if (Configs.Select(x => x.Name).Where(y => y == NewConfigName).Any())
            {
                MessageBox.Show("Config with that name already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return;
            }
            AddNewConfig();
        }

        void AddNewConfig()
        {
            var vm = new AlertConfigViewModel(EventAggregator) { Id = IdProvider.GetId(), ConfigName = NewConfigName };
            vm.AddNewAlert.Execute();
            var v = new AlertConfigView { DataContext = vm };
            var config = new DropdownConfig { Config = v, Name = NewConfigName };
            Configs.Add(config);
            NewConfigName = "";
            EventAggregator.GetEvent<AlertConfigAdded>().Publish();
        }

        void OnSaveConfig(AlertConfig config)
        {
            foreach (var cfg in Configs)
            {
                var vm = cfg.Config.DataContext as AlertConfigViewModel;
                if (vm.Id == config.Id)
                    cfg.Name = config.Name;
            }
            Store.StoreObject(config, config.Id);
            var c = Store.GetObject<StoredAlertConfigs>();
            if (c == null)
                c = new StoredAlertConfigs();
            if (!c.AlertConfigs.Contains(config.Id))
                c.AlertConfigs.Add(config.Id);
            Store.StoreObject(c);
        }

        void OnDeleteConfig(DeleteAlertConfigArgs args)
        {
            foreach (var config in Configs)
            {
                var vm = config.Config.DataContext as AlertConfigViewModel;
                if (vm.Id == args.Id)
                {
                    Configs.Remove(config);
                    Store.RemoveObject(config.Name);
                    return;
                }
            }
        }
    }

    public class DropdownConfig : BindableBase
    {
        string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        Control config;
        public Control Config
        {
            get => config;
            set => SetProperty(ref config, value);
        }
    }
}
