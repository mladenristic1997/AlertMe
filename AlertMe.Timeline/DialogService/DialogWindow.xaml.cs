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

namespace AlertMe.Timeline
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        public DialogWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SizeToContent = SizeToContent.WidthAndHeight;
            InitializeComponent();
        }

        public void SetChildren(Control child)
        {
            ContentGrid.Children.Add(child);
        }

        public void CloseDialog()
        {
            ContentGrid.Children.Clear();
            Close();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            CloseDialog();
        }
    }
}
