using AlertMe.Domain;
using Prism.Events;

namespace AlertMe.Config.Events
{
    public class AlertChanged : PubSubEvent<AlertChangedArgs>
    {
    }

    public class AlertChangedArgs
    {
        public Alert Alert { get; set; } 
    }
}
