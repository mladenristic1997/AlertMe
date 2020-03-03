using AlertMe.Config.Commands;
using AlertMe.Config.Events;
using AlertMe.Domain;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlertMe.Config
{
    public class AlertViewModel : BindableBase
    {
        readonly IEventAggregator EventAggregator;

        public string Id { get; set; }

        public DelegateCommand Remove { get; set; }

        public List<AlertType> AlertTypes { get; set; }

        AlertType selectedType;
        public AlertType SelectedType
        {
            get => selectedType;
            set 
            {
                SetProperty(ref selectedType, value);
                Alert.AlertType = SelectedType;
            }
        }

        Alert alert;
        public Alert Alert
        {
            get => alert;
            set 
            {
                SetProperty(ref alert, value);
                NotifyChange();
            }
        }

        public AlertViewModel(IEventAggregator ea)
        {
            EventAggregator = ea;
            AlertTypes = new List<AlertType>();
            foreach (var at in (AlertType[])Enum.GetValues(typeof(AlertType)))
                AlertTypes.Add(at);
            Remove = new DelegateCommand(OnRemoveAlert);
        }

        void OnRemoveAlert()
        {
            EventAggregator.GetEvent<RemoveAlert>().Publish(new RemoveAlertArgs { Id = Id });
        }

        void NotifyChange()
        {
            EventAggregator.GetEvent<AlertChanged>().Publish(new AlertChangedArgs { Alert = Alert });
        }
    }
}
