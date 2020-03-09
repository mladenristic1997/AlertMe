using AlertMe.Home.Commands;
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

namespace AlertMe.Home
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        readonly IEventAggregator EventAggregator;
        
        public HomeView(IEventAggregator ea)
        {
            EventAggregator = ea;
            InitializeComponent();
            EventAggregator.GetEvent<LoadPlans>().Publish();
            EventAggregator.GetEvent<RefreshSelection>().Subscribe(OnRefreshSelection);
        }

        void OnRefreshSelection()
        {
            var selected = Plans.SelectedIndex;
            Plans.SelectedIndex = -1;
            Plans.SelectedIndex = Plans.Items.Count > 0 ? selected : -1;
        }
    }
}
