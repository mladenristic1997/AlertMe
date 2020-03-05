using Prism.Commands;
using Prism.Mvvm;

namespace AlertMe.AlertSoundSelector
{
    public class RecordNewViewModel : BindableBase
    {
        public DelegateCommand ToggleRecord { get; set; }

        bool isRecording;
        public bool IsRecording
        {
            get => isRecording;
            set => SetProperty(ref isRecording, value);
        }

        public RecordNewViewModel()
        {
            ToggleRecord = new DelegateCommand(OnToggleRecord);
        }

        void OnToggleRecord()
        {
            //record sound
        }
    }
}
