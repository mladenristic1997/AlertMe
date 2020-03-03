using System;
using System.Collections.Generic;
using System.Windows;

namespace AlertMe.Timeline
{
    public partial class Timeline
    {
        public static readonly DependencyProperty AlertsProperty =
            DependencyProperty.Register("Alerts", typeof(IList<Alert>), typeof(Timeline), new PropertyMetadata(default(IList<Alert>)));

        public IList<Alert> Alerts
        {
            get => (IList<Alert>)GetValue(AlertsProperty);
            set => SetValue(AlertsProperty, value);
        }

        public static readonly DependencyProperty PercentagePassedProperty =
            DependencyProperty.Register("PercentagePassed", typeof(double), typeof(Timeline), new PropertyMetadata(default(double)));

        public double PercentagePassed
        {
            get => double.Parse(GetValue(PercentagePassedProperty).ToString());
            set => SetValue(PercentagePassedProperty, value);
        }
    }
}
