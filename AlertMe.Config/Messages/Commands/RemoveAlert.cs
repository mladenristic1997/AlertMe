using Prism.Events;

namespace AlertMe.Config.Commands
{
    public class RemoveAlert : PubSubEvent<RemoveAlertArgs>
    {
    }

    public class RemoveAlertArgs
    {
        public string Id { get; set; }
    }
}
