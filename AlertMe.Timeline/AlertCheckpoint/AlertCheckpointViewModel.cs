using System.Windows;

namespace AlertMe.Timeline.AlertCheckpoint
{
    public class AlertCheckpointViewModel : BindableBase
    {

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

        Thickness marginLeft;
        public Thickness MarginLeft
        {
            get => marginLeft;
            set => SetProperty(ref marginLeft, value);
        }

        double percentagePosition;
        public double PercentagePosition
        {
            get => percentagePosition;
            set
            {
                SetProperty(ref percentagePosition, value);
                UpdateMargin();
            }
        }

        double timelineWidth;
        public double TimelineWidth
        {
            get => timelineWidth;
            set
            {
                SetProperty(ref timelineWidth, value);
                UpdateMargin();
            }
        }

            void UpdateMargin()
            {
                var left = PercentagePosition * TimelineWidth;
                MarginLeft = new Thickness(left, 0, 0, 0);
            }
    }
}
