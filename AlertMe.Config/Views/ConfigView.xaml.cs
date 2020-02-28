using AlertMe.Config.Commands;
using AlertMe.Config.Events;
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

namespace AlertMe.Config
{
    /// <summary>
    /// Interaction logic for ConfigView.xaml
    /// </summary>
    public partial class ConfigView : UserControl
    {
        readonly IEventAggregator EventAggregator;

        public ConfigView(IEventAggregator ea)
        {
            EventAggregator = ea;
            InitializeComponent();
            EventAggregator.GetEvent<AlertConfigAdded>().Subscribe(UpdateComboBox);
            EventAggregator.GetEvent<DeleteAlertConfig>().Subscribe(OnDeleteConfig);
        }

        void UpdateComboBox()
        {
            AlertConfigs.SelectedIndex = AlertConfigs.Items.Count - 1;
        }

        void OnDeleteConfig(DeleteAlertConfigArgs e)
        {
            AlertConfigs.SelectedIndex = AlertConfigs.Items.Count > 0 ? 0 : -1;
        }
    }
}
