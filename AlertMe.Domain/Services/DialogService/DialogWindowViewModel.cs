using AlertMe.Domain.Commands;

namespace AlertMe.Domain
{
    public class DialogWindowViewModel : BindableBase
    {
        readonly IEventAggregator EventAggregator;

        public DelegateCommand CloseWindow { get; set; }

        public DialogWindowViewModel(IEventAggregator ea)
        {
            EventAggregator = ea;
            CloseWindow = new DelegateCommand(OnCloseWindow);
        }

        void OnCloseWindow()
        {
            EventAggregator.GetEvent<CloseDialog>().Publish();
        }
    }
}
