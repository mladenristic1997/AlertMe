using AlertMe.Plans.Commands;
using AlertMe.Plans.Events;
using AlertMe.Domain;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AlertMe.Plans
{
    public class PlansViewModel : BindableBase
    {
        readonly IEventAggregator EventAggregator;
        readonly ILocalDataStore Store;

        public DelegateCommand AddNewPlanCommand { get; set; }

        string newPlanName;
        public string NewPlanName
        {
            get => newPlanName;
            set => SetProperty(ref newPlanName, value);
        }

        public ObservableCollection<DropdownPlan> Plans { get; set; }

        DropdownPlan selectedPlan;
        public DropdownPlan SelectedPlan
        {
            get => selectedPlan;
            set => SetProperty(ref selectedPlan, value);
        }

        public PlansViewModel(IEventAggregator ea, ILocalDataStore store)
        {
            EventAggregator = ea;
            Store = store;
            Plans = new ObservableCollection<DropdownPlan>();
            AddNewPlanCommand = new DelegateCommand(OnAddNewPlan);
            EventAggregator.GetEvent<SaveAlertPlan>().Subscribe(OnSavePlan);
            EventAggregator.GetEvent<DeleteAlertPlan>().Subscribe(OnDeletePlan);
            EventAggregator.GetEvent<LoadPlans>().Subscribe(LoadStoredPlans);
        }

        void LoadStoredPlans()
        {
            var c = Store.GetObject<StoredAlertPlans>();
            if (c != null)
            {
                foreach (var cfg in c.AlertPlans)
                {
                    var cf = Store.GetObject<AlertPlan>(cfg);
                    var alertsList = new ObservableCollection<Control>();
                    var timelineAlerts = new ObservableCollection<Timeline.Alert>();
                    foreach (var alert in cf.Alerts)
                    {
                        var avm = new AlertViewModel(EventAggregator) { 
                            Id = alert.Id,
                            PlanId = cf.Id,
                            Hours = alert.Hours,
                            Minutes = alert.Minutes,
                            Seconds = alert.Seconds,
                            Message = alert.Message
                        };
                        alertsList.Add(new AlertView { DataContext = avm });
                        var ta = new Timeline.Alert
                        {
                            Id = alert.Id,
                            Message = alert.Message,
                            TotalSeconds = CalculateInSeconds(alert.Hours, alert.Minutes, alert.Seconds)
                        };
                        timelineAlerts.Add(ta);
                    }
                    var vm = new AlertPlanViewModel(EventAggregator) { PlanName = cf.Name, Id = cf.Id, Alerts = alertsList, TimelineAlerts = timelineAlerts, PlanDuration = timelineAlerts.Sum(x => x.TotalSeconds) };
                    Plans.Add(new DropdownPlan { Name = cf.Name, Plan = new AlertPlanView { DataContext = vm } });
                    EventAggregator.GetEvent<PlansLoaded>().Publish();
                }
            }
        }

            int CalculateInSeconds(int h, int m, int s) => h * 60 * 60 + m * 60 + s;


        void OnAddNewPlan()
        {
            if (Plans.Select(x => x.Name).Where(y => y == NewPlanName).Any())
            {
                MessageBox.Show("Config with that name already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return;
            }
            AddNewPlan();
        }

        void AddNewPlan()
        {
            var vm = new AlertPlanViewModel(EventAggregator) { Id = IdProvider.GetId(), PlanName = NewPlanName };
            vm.AddNewAlert.Execute();
            var v = new AlertPlanView { DataContext = vm };
            var plan = new DropdownPlan { Plan = v, Name = NewPlanName };
            Plans.Add(plan);
            NewPlanName = "";
            EventAggregator.GetEvent<AlertPlanAdded>().Publish();
        }

        void OnSavePlan(AlertPlan config)
        {
            foreach (var cfg in Plans)
            {
                var vm = cfg.Plan.DataContext as AlertPlanViewModel;
                if (vm.Id == config.Id)
                    cfg.Name = config.Name;
            }
            Store.StoreObject(config, config.Id);
            var c = Store.GetObject<StoredAlertPlans>();
            if (c == null)
                c = new StoredAlertPlans();
            if (!c.AlertPlans.Contains(config.Id))
                c.AlertPlans.Add(config.Id);
            Store.StoreObject(c);
        }

        void OnDeletePlan(DeleteAlertPlanArgs args)
        {
            foreach (var config in Plans)
            {
                var vm = config.Plan.DataContext as AlertPlanViewModel;
                if (vm.Id == args.Id)
                {
                    Plans.Remove(config);
                    Store.RemoveObject(vm.Id);
                    var c = Store.GetObject<StoredAlertPlans>();
                    c.AlertPlans.Remove(vm.Id);
                    Store.StoreObject(c);
                    return;
                }
            }
        }
    }

    public class DropdownPlan : BindableBase
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
