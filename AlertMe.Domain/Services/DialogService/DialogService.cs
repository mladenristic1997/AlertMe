using System.Windows;
using System.Windows.Controls;

namespace AlertMe.Domain
{
    public class DialogService
    {
        readonly IEventAggregator EventAggregator;

        public DialogService(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
        }

        public void Show(Control dialogContent)
        {
            var v = new DialogWindow(EventAggregator) { DataContext = new DialogWindowViewModel(EventAggregator) };
            v.Owner = Application.Current.MainWindow;
            v.SetChildren(dialogContent);
            v.ShowDialog();
        }
    }
}
