using Prism.Mvvm;
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

        Thickness margin;
        public Thickness Margin
        {
            get => margin;
            set => SetProperty(ref margin, value);
        }

        public double LeftMargin { get; set; }
    }
}
