using Prism.Events;

namespace AlertMe.Config.Events
{
    public class AlertChanged : PubSubEvent<AlertChangedArgs>
    {
    }

    public class AlertChangedArgs
    {
        public string Id { get; set; }
    }
}
