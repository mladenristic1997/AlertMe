using AlertMe.AlertSoundSelector.Events;
using AlertMe.Domain.Commands;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace AlertMe.AlertSoundSelector
{
    public class RecordNewViewModel : BindableBase
    {
        readonly IEventAggregator EventAggregator;

        public DelegateCommand StartRecording { get; set; }
        public DelegateCommand StopRecording { get; set; }
        public DelegateCommand RemoveRecording { get; set; }
        public DelegateCommand SaveRecording { get; set; }

        string statusText;
        public string StatusText
        {
            get => statusText;
            set => SetProperty(ref statusText, value);
        }

        bool isRecording;
        public bool IsRecording
        {
            get => isRecording;
            set => SetProperty(ref isRecording, value);
        }

        bool isIdle;
        public bool IsIdle
        {
            get => isIdle;
            set => SetProperty(ref isIdle, value);
        }

        bool hasRecorded;
        public bool HasRecorded
        {
            get => hasRecorded;
            set => SetProperty(ref hasRecorded, value);
        }

        string AlertId { get; set; }
        string PlanId { get; set; }

        public RecordNewViewModel(IEventAggregator ea)
        {
            EventAggregator = ea;
            EventAggregator.GetEvent<OpenAlertSoundSelector>().Subscribe(OnOpen);
            StartRecording = new DelegateCommand(OnStartRecording);
            StopRecording = new DelegateCommand(OnStopRecording);
            RemoveRecording = new DelegateCommand(OnRemoveRecording);
            SaveRecording = new DelegateCommand(OnSaveRecording);
            EventAggregator.GetEvent<AlertSoundSelectorClosed>().Subscribe(OnClose);
            StatusText = "Record new";
        }

        void OnOpen(OpenAlertSoundSelectorArgs e) 
        { 
            AlertId = e.AlertId; 
            PlanId = e.PlanId;
            IsIdle = true;
        }

        void OnClose()
        {
            if (IsRecording)
                OnStopRecording();
            HasRecorded = false;
            //null the recorded value
        }

        void OnStartRecording()
        {
            IsRecording = true;
            IsIdle = false;
            StatusText = "Recording";
        }

        void OnStopRecording()
        {
            IsRecording = false;
            IsIdle = true;
            HasRecorded = true;
            StatusText = "Record new";
        }

        void OnRemoveRecording()
        {
            HasRecorded = false;

        }

        void OnSaveRecording()
        {
            HasRecorded = false;
            //notify of success
        }
    }
}
