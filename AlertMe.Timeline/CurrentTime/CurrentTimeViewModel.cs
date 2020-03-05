using Prism.Mvvm;
using System.Windows;

namespace AlertMe.Timeline.CurrentTime
{
    public class CurrentTimeViewModel : BindableBase
    {
        Thickness marginLeft;
        public Thickness MarginLeft
        {
            get => marginLeft;
            set => SetProperty(ref marginLeft, value);
        }

        double percentagePassed;
        public double PercentagePassed
        {
            get => percentagePassed;
            set
            {
                SetProperty(ref percentagePassed, value);
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
            var left = PercentagePassed * TimelineWidth;
            MarginLeft = new Thickness(left, 0, 0, 0);
        }
    }
}
