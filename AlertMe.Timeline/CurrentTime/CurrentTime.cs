using System;
using System.Windows;
using System.Windows.Controls;

namespace AlertMe.Timeline.CurrentTime
{
    public partial class CurrentTime : UserControl
    {
        public static readonly DependencyProperty PercentagePassedProperty =
            DependencyProperty.Register("PercentagePassed", typeof(double), typeof(CurrentTime), new FrameworkPropertyMetadata()
            {
                PropertyChangedCallback = OnPercentagePassedChanged,
                BindsTwoWayByDefault = false
            });

        static void OnPercentagePassedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var vm = d as CurrentTime;
            vm.ViewModel.PercentagePassed = Convert.ToDouble(e.NewValue);
        }

        public double PercentagePassed
        {
            get => double.Parse(GetValue(PercentagePassedProperty).ToString());
            set => SetValue(PercentagePassedProperty, value);
        }

        public static readonly DependencyProperty TimelineWidthProperty =
            DependencyProperty.Register("TimelineWidth", typeof(double), typeof(CurrentTime), new FrameworkPropertyMetadata()
            {
                PropertyChangedCallback = OnTimelineWidthChanged,
                BindsTwoWayByDefault = false
            });

        static void OnTimelineWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var vm = d as CurrentTime;
            vm.ViewModel.TimelineWidth = Convert.ToDouble(e.NewValue);
        }

        public double TimelineWidth
        {
            get => double.Parse(GetValue(TimelineWidthProperty).ToString());
            set => SetValue(TimelineWidthProperty, value);
        }
    }
}
