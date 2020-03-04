using System.Windows;
using System.Windows.Controls;

namespace AlertMe.Timeline
{
    public class DialogService
    {
        Control CurrentView;

        public void Show(Control dialogContent)
        {
            CloseDialog();
            var v = new DialogWindow() { };
            v.Owner = Application.Current.MainWindow;
            v.SetChildren(dialogContent);
            CurrentView = v;
            v.Show();
        }

        public void CloseDialog()
        {
            var cv = CurrentView as DialogWindow;
            if (cv != null)
                cv.CloseDialog();
        }
    }
}
