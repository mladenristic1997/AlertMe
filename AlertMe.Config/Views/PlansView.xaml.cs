using AlertMe.Domain.Commands;
using AlertMe.Domain.Events;
using AlertMe.Plans.Commands;
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

namespace AlertMe.Plans
{
    /// <summary>
    /// Interaction logic for ConfigView.xaml
    /// </summary>
    public partial class PlansView : UserControl
    {
        readonly IEventAggregator EventAggregator;

        public PlansView(IEventAggregator ea)
        {
            EventAggregator = ea;
            InitializeComponent();
            EventAggregator.GetEvent<AlertPlanAdded>().Subscribe(UpdateComboBox);
            EventAggregator.GetEvent<PlansLoaded>().Subscribe(OnPlansLoaded);
            EventAggregator.GetEvent<DeleteAlertPlan>().Subscribe(OnDeleteConfig);
            EventAggregator.GetEvent<LoadPlans>().Publish();
        }

        void UpdateComboBox()
        {
            AlertConfigs.SelectedIndex = AlertConfigs.Items.Count - 1;
        }

        void OnPlansLoaded()
        {
            AlertConfigs.SelectedIndex = AlertConfigs.Items.Count > 0 ? 0 : -1;
        }

        void OnDeleteConfig(DeleteAlertPlanArgs e)
        {
            AlertConfigs.SelectedIndex = AlertConfigs.Items.Count > 0 ? 0 : -1;
        }
    }
}
