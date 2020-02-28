using Prism.Events;

namespace AlertMe.Config.Commands
{
    public class DeleteAlertConfig : PubSubEvent<DeleteAlertConfigArgs>
    {
    }

    public class DeleteAlertConfigArgs
    {
        public string Id { get; set; }
    }
}
