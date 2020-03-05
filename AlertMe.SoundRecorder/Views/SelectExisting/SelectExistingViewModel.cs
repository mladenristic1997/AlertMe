using AlertMe.AlertSoundSelector.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace AlertMe.AlertSoundSelector
{
    public class SelectExistingViewModel : BindableBase
    {
        readonly IEventAggregator EventAggregator;

        public DelegateCommand SelectSound { get; set; }

        public SelectExistingViewModel(IEventAggregator ea)
        {
            EventAggregator = ea;
            EventAggregator.GetEvent<AlertSoundSelected>().Publish();
            SelectSound = new DelegateCommand(OnSelectSound);
        }

        void OnSelectSound()
        {
            //open file picker dialog and choose mp3 or wav
        }
    }
}
