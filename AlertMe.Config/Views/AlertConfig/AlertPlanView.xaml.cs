using AlertMe.Plans.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AlertMe.Plans
{
    /// <summary>
    /// Interaction logic for ConfigView.xaml
    /// </summary>
    public partial class AlertPlanView : UserControl
    {
        readonly IEventAggregator EventAggregator;
        public AlertPlanView(IEventAggregator ea)
        {
            EventAggregator = ea;
            EventAggregator.GetEvent<FinishedViewModelUpdate>().Subscribe(OnUpdate);
            InitializeComponent();
        }

        void OnUpdate()
        {
            TimelineControl.Dispatcher.Invoke(() => { }, DispatcherPriority.Render);
        }
    }
}
