using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace AlertMe.Timeline
{
    public partial class Timeline : UserControl
    {
        public static readonly DependencyProperty AlertsProperty =
            DependencyProperty.Register("Alerts", typeof(ObservableCollection<Alert>), typeof(Timeline), new FrameworkPropertyMetadata()
            {
                PropertyChangedCallback = OnAlertsChanged,
            });

        static void OnAlertsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var vm = d as Timeline;
            vm.ViewModel.Alerts = (ObservableCollection<Alert>)e.NewValue;
            foreach (var a in vm.ViewModel.Alerts)
                a.Update = vm.ViewModel.UpdateView;
            vm.ViewModel.UpdateView();
        }

        public ObservableCollection<Alert> Alerts
        {
            get => (ObservableCollection<Alert>)GetValue(AlertsProperty);
            set => SetValue(AlertsProperty, value);
        }



        public static readonly DependencyProperty ControlWidthProperty =
            DependencyProperty.Register("PlanDuration", typeof(int), typeof(Timeline), new FrameworkPropertyMetadata()
            {
                PropertyChangedCallback = OnControlWidthChanged,
            });

        static void OnControlWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var vm = d as Timeline;
            vm.ViewModel.ControlWidth = Convert.ToInt32(e.NewValue);
        }

        public int ControlWidth
        {
            get => int.Parse(GetValue(ControlWidthProperty).ToString());
            set => SetValue(ControlWidthProperty, value);
        }




        public static readonly DependencyProperty PlanDurationProperty =
            DependencyProperty.Register("PlanDuration", typeof(int), typeof(Timeline), new FrameworkPropertyMetadata()
            {
                PropertyChangedCallback = OnPlanDurationChanged,
            });

        static void OnPlanDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var vm = d as Timeline;
            vm.ViewModel.PlanDuration = Convert.ToInt32(e.NewValue);
        }

        public int PlanDuration
        {
            get => int.Parse(GetValue(PlanDurationProperty).ToString());
            set => SetValue(PlanDurationProperty, value);
        }
    }
}
