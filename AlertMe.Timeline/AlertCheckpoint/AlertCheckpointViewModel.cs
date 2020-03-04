using System.Windows;

namespace AlertMe.Timeline.AlertCheckpoint
{
    public class AlertCheckpointViewModel : BindableBase
    {
        string id;
        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        string message;
        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        string alertAt;
        public string AlertAt
        {
            get => alertAt;
            set => SetProperty(ref alertAt, value);
        }

        Thickness Margin;
        public Thickness Margin
        {
            get => marginLeft;
            set => SetProperty(ref marginLeft, value);
        }
    }
}
