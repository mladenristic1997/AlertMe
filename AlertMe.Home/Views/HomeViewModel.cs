using AlertMe.Domain;
using AlertMe.Domain.Events;
using AlertMe.Home.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            Store = store;
            EventAggregator.GetEvent<LoadPlans>().Subscribe(LoadStoredPlans);
            EventAggregator.GetEvent<LocalStoreChanged>().Subscribe(LoadStoredPlans);
        }
        
        void LoadStoredPlans()
        {
            var c = Store.GetObject<StoredAlertPlans>();
            if (c != null)
            {
                AvailablePlans = new List<AvailableDropdownPlan>();
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
                    AvailablePlans.Add(new AvailableDropdownPlan { Name = cf.Name, Plan = new PlanView { DataContext = vm } });
                }
            }
        }

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
