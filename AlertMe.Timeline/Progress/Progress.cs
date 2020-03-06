using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace AlertMe.Timeline
{
    public partial class Progress : UserControl
    {
        public static readonly DependencyProperty AlertsProperty =
            DependencyProperty.Register("Alerts", typeof(ObservableCollection<Alert>), typeof(Progress), new FrameworkPropertyMetadata()
            {
                PropertyChangedCallback = OnAlertsChanged,
                BindsTwoWayByDefault = true
            });

        static void OnAlertsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var vm = d as Progress;
            vm.ViewModel.Alerts = (ObservableCollection<Alert>)e.NewValue;
            foreach (var a in vm.ViewModel.Alerts)
                a.Update = vm.ViewModel.UpdateView;
            vm.ViewModel.UpdateView();
            vm.ViewModel.AssignAlertTimes();
            vm.ViewModel.UpdateNext();
        }

        public ObservableCollection<Alert> Alerts
        {
            get => (ObservableCollection<Alert>)GetValue(AlertsProperty);
            set => SetValue(AlertsProperty, new ObservableCollection<Alert>());
        }



        public static readonly DependencyProperty ControlWidthProperty =
            DependencyProperty.Register("ControlWidth", typeof(int), typeof(Progress), new FrameworkPropertyMetadata()
            {
                PropertyChangedCallback = OnControlWidthChanged,
            });

        static void OnControlWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var vm = d as Progress;
            vm.ViewModel.ControlWidth = Convert.ToInt32(e.NewValue);
        }

        public int ControlWidth
        {
            get => int.Parse(GetValue(ControlWidthProperty).ToString());
            set => SetValue(ControlWidthProperty, value);
        }




        public static readonly DependencyProperty PlanDurationProperty =
            DependencyProperty.Register("PlanDuration", typeof(int), typeof(Progress), new FrameworkPropertyMetadata()
            {
                PropertyChangedCallback = OnPlanDurationChanged,
            });

        static void OnPlanDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var vm = d as Progress;
            vm.ViewModel.PlanDuration = Convert.ToInt32(e.NewValue);
        }

        public int PlanDuration
        {
            get => int.Parse(GetValue(PlanDurationProperty).ToString());
            set => SetValue(PlanDurationProperty, value);
        }
    }
}
