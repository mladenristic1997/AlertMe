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

        string AlertId { get; set; }
        string PlanId { get; set; }

        public RecordNewViewModel(IEventAggregator ea)
        {
            EventAggregator = ea;
            EventAggregator.GetEvent<OpenAlertSoundSelector>().Subscribe(e => { AlertId = e.AlertId; PlanId = e.PlanId; });
            StartRecording = new DelegateCommand(OnStartRecording);
            StopRecording = new DelegateCommand(OnStopRecording);
            StatusText = "Record new";
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
            StatusText = "Record new";
        }
    }
}
