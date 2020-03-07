using Prism.Commands;
using Prism.Mvvm;

namespace AlertMe.AlertSoundSelector
{
    public class RecordNewViewModel : BindableBase
    {
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

        public RecordNewViewModel()
        {
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
