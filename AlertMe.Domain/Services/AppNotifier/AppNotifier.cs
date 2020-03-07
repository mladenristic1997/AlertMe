using AlertMe.Domain.Events;
using Prism.Events;
using System;
using System.Windows;
using PeanutButter.Toast;

namespace AlertMe.Domain
{
    public class AppNotifier
    {
        readonly IEventAggregator EventAggregator;
        Toaster Notifier;
        Rect Position;
        public AppNotifier(IEventAggregator ea)
        {
            EventAggregator = ea;
            EventAggregator.GetEvent<ApplicationErrorOccured>().Subscribe(Notify);
            EventAggregator.GetEvent<ApplicationSuccessOccured>().Subscribe(Notify);
            Notifier = new Toaster();
        }

        public void Notify(dynamic msg)
        {
            When(msg);
        }

        void When(ApplicationErrorOccuredArgs msg)
        {
            Position = new Rect(
                Application.Current.MainWindow.Left,
                Application.Current.MainWindow.Top,
                Application.Current.MainWindow.ActualWidth,
                Application.Current.MainWindow.ActualHeight
                );
            Notifier.Show("Operation failed", msg.Error, ToastTypes.Error, Position);
        }

        void When(ApplicationSuccessOccuredArgs msg)
        {
            Position = new Rect(
                            Application.Current.MainWindow.Left,
                            Application.Current.MainWindow.Top,
                            Application.Current.MainWindow.ActualWidth,
                            Application.Current.MainWindow.ActualHeight
                            );
            Notifier.Show("Success", msg.Message, ToastTypes.Success, Position);
        }
    }
}
