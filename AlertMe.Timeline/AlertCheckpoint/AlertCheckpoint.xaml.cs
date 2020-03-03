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

namespace AlertMe.Timeline.AlertCheckpoint
{
    /// <summary>
    /// Interaction logic for AlarmCheckpoint.xaml
    /// </summary>
    public partial class AlertCheckpoint : UserControl
    {
        public AlertCheckpointViewModel ViewModel { get; set; }

        public AlertCheckpoint()
        {
            ViewModel = new AlertCheckpointViewModel();
            InitializeComponent();
            LayoutRoot.DataContext = ViewModel;
        }

        private void LayoutRoot_MouseEnter(object sender, MouseEventArgs e)
        {
            MessageBox.Visibility = Visibility.Collapsed;
        }

        private void LayoutRoot_MouseLeave(object sender, MouseEventArgs e)
        {
            MessageBox.Visibility = Visibility.Collapsed;
        }
    }
}
