using AlertMe.Domain;
using AlertMe.Domain.Events;
using AlertMe.Home.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace AlertMe.Home
{
    public class HomeViewModel : BindableBase
    {
        readonly IEventAggregator EventAggregator;
        readonly ILocalDataStore Store;

        public ObservableCollection<DropdownPlan> Plans { get; set; }

        DropdownPlan selectedPlan;
        public DropdownPlan SelectedPlan
        {
            get => selectedPlan;
            set => SetProperty(ref selectedPlan, value);
        }

        public HomeViewModel(IEventAggregator ea, ILocalDataStore store)
        {
            EventAggregator = ea;
            Store = store;
            Plans = new ObservableCollection<DropdownPlan>();
            EventAggregator.GetEvent<LoadPlans>().Subscribe(LoadStoredPlans);
            EventAggregator.GetEvent<LocalStoreChanged>().Subscribe(LoadStoredPlans);
        }
        
        void LoadStoredPlans()
        {
            var c = Store.GetObject<StoredAlertPlans>();
            if (c != null)
            {
                Plans = new ObservableCollection<DropdownPlan>();
                foreach (var cfg in c.AlertPlans)
                {
                    var cf = Store.GetObject<AlertPlan>(cfg);
                    var timelineAlerts = new ObservableCollection<Timeline.Alert>();
                    foreach (var alert in cf.Alerts)
                    {
                        var ta = new Timeline.Alert
                        {
                            Id = alert.Id,
                            Message = alert.Message,
                            TotalSeconds = CalculateInSeconds(alert.Hours, alert.Minutes, alert.Seconds)
                        };
                        timelineAlerts.Add(ta);
                    }
                    var vm = new PlanViewModel() { TimelineAlerts = timelineAlerts, PlanDuration = timelineAlerts.Sum(x => x.TotalSeconds) };
                    Plans.Add(new DropdownPlan { Name = cf.Name, Plan = new PlanView { DataContext = vm } });
                }
            }
        }

            int CalculateInSeconds(int h, int m, int s) => h * 60 * 60 + m * 60 + s;
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
