using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace AlertMe.Home
{
    public class PlanViewModel : BindableBase
    {
        public int PlanDuration { get; set; }

        public ObservableCollection<Timeline.Alert> TimelineAlerts { get; set; }

        public PlanViewModel()
        {
            TimelineAlerts = new ObservableCollection<Timeline.Alert>();
        }
    }
}
