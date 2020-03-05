using AlertMe.Domain.Commands;
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

namespace AlertMe.Domain
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        readonly IEventAggregator EventAggregator;

        public DialogWindow(IEventAggregator eventAggregator)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SizeToContent = SizeToContent.WidthAndHeight;
            EventAggregator = eventAggregator;
            EventAggregator.GetEvent<CloseDialog>().Subscribe(OnCloseDialogRequested);
            InitializeComponent();
        }

        public void SetChildren(Control child)
        {
            ContentGrid.Children.Add(child);
        }

        void OnCloseDialogRequested()
        {
            EventAggregator.GetEvent<CloseDialog>().Unsubscribe(OnCloseDialogRequested);
            ContentGrid.Children.Clear();
            Close();
        }
    }
}
