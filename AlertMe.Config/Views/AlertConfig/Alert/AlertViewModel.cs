using AlertMe.Plans.Commands;
using AlertMe.Plans.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace AlertMe.Plans
{
    public class AlertViewModel : BindableBase
    {
        readonly IEventAggregator EventAggregator;

        public string Id { get; set; }

        public DelegateCommand Remove { get; set; }

        int hours;
        public int Hours
        {
            get => hours;
            set
            {
                SetProperty(ref hours, value);
                NotifyChange();
            }
        }

        int minutes;
        public int Minutes 
        {
            get => minutes;
            set
            {
                SetProperty(ref minutes, value);
                NotifyChange();
            }
        }

        int seconds;
        public int Seconds
        {
            get => seconds;
            set
            {
                SetProperty(ref seconds, value);
                NotifyChange();
            }
        }

        string message;
        public string Message
        {
            get => message;
            set
            {
                SetProperty(ref message, value);
                NotifyChange();
            }
        }

        public AlertViewModel(IEventAggregator ea)
        {
            EventAggregator = ea;
            Remove = new DelegateCommand(OnRemoveAlert);
        }

        void OnRemoveAlert()
        {
            EventAggregator.GetEvent<RemoveAlert>().Publish(new RemoveAlertArgs { Id = Id });
        }

        void NotifyChange()
        {
            EventAggregator.GetEvent<AlertChanged>().Publish(new AlertChangedArgs { 
                Id = Id,
                Hours = Hours,
                Minutes = Minutes,
                Seconds = Seconds,
                Message = Message
            });
        }
    }
}
