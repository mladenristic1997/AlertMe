using System;
using System.Windows;
using System.Windows.Controls;

namespace AlertMe.Timeline.AlertCheckpoint
{
    public partial class AlertCheckpoint : UserControl
    {
        public static readonly DependencyProperty AlertAtProperty =
            DependencyProperty.Register("AlertAt", typeof(string), typeof(AlertCheckpoint), new FrameworkPropertyMetadata()
            {
                PropertyChangedCallback = OnAlertAtPropertyChanged,
                BindsTwoWayByDefault = false,
            });

        static void OnAlertAtPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var vm = d as AlertCheckpoint;
            vm.ViewModel.AlertAt = e.NewValue.ToString();
        }

        public string AlertAt
        {
            get => GetValue(AlertAtProperty).ToString();
            set => SetValue(AlertAtProperty, value);
        }


        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(AlertCheckpoint), new FrameworkPropertyMetadata()
            {
                PropertyChangedCallback = OnMessagePropertyChanged,
                BindsTwoWayByDefault = false,
            });

        static void OnMessagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var vm = d as AlertCheckpoint;
            vm.ViewModel.Message = e.NewValue.ToString();
        }

        public string Message
        {
            get => GetValue(MessageProperty).ToString();
            set => SetValue(MessageProperty, value);
        }


        public static readonly DependencyProperty PercentagePositionProperty =
            DependencyProperty.Register("PercentagePosition", typeof(double), typeof(AlertCheckpoint), new FrameworkPropertyMetadata()
            {
                PropertyChangedCallback = OnPercentagePositionChanged,
                BindsTwoWayByDefault = false,
            });

        static void OnPercentagePositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var vm = d as AlertCheckpoint;
            vm.ViewModel.PercentagePosition = Convert.ToDouble(e.NewValue);
        }

        public double PercentagePosition
        {
            get => double.Parse(GetValue(PercentagePositionProperty).ToString());
            set => SetValue(PercentagePositionProperty, value);
        }


        public static readonly DependencyProperty TimelineWidthProperty =
            DependencyProperty.Register("TimelineWidth", typeof(double), typeof(AlertCheckpoint), new FrameworkPropertyMetadata()
            {
                PropertyChangedCallback = OnTimelineWidthChanged,
                BindsTwoWayByDefault = false
            });

        static void OnTimelineWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var vm = d as AlertCheckpoint;
            vm.ViewModel.TimelineWidth = Convert.ToDouble(e.NewValue);
        }

        public double TimelineWidth
        {
            get => double.Parse(GetValue(TimelineWidthProperty).ToString());
            set => SetValue(TimelineWidthProperty, value);
        }
    }
}
