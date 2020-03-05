using System.Windows;
using System.Windows.Controls;

namespace AlertMe.Timeline.AlertCheckpoint
{
    public partial class AlertCheckpoint : UserControl
    {
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(string), typeof(AlertCheckpoint), new FrameworkPropertyMetadata()
            {
                PropertyChangedCallback = OnIdChanged,
                BindsTwoWayByDefault = false,
            });

        static void OnIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var vm = d as AlertCheckpoint;
            vm.ViewModel.Id = e.NewValue.ToString();
        }

        public string Id
        {
            get => GetValue(IdProperty).ToString();
            set => SetValue(IdProperty, value);
        }




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




        public static readonly DependencyProperty LeftMarginProperty =
            DependencyProperty.Register("LeftMargin", typeof(double), typeof(AlertCheckpoint), new FrameworkPropertyMetadata()
            {
                PropertyChangedCallback = OnLeftMarginChanged,
                BindsTwoWayByDefault = false,
            });

        static void OnLeftMarginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var vm = d as AlertCheckpoint;
            vm.ViewModel.Margin = new Thickness(double.Parse(e.NewValue.ToString()), 0, 0, 0);
        }

        public double LeftMargin
        {
            get => double.Parse(GetValue(LeftMarginProperty).ToString());
            set => SetValue(LeftMarginProperty, value);
        }
    }
}
