using System.Windows;
using System.Windows.Controls;

namespace AlertMe.Domain
{
    public interface IDialogService
    {
        void Show(Control control);
    }
    public class DialogService : IDialogService
    {
        public void Show(Control dialogContent)
        {
            var v = new DialogWindow();
            v.Owner = Application.Current.MainWindow;
            v.SetChildren(dialogContent);
            v.Show();
        }
    }
}
