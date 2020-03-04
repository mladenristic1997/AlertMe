using AlertMe.Domain;
using Prism.Events;

namespace AlertMe.Plans.Events
{
    public class AlertChanged : PubSubEvent<AlertChangedArgs>
    {
    }

    public class AlertChangedArgs
    {
        public string Id { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public string Message { get; set; }
    }
}
