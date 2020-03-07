using Prism.Events;

namespace AlertMe.Domain.Events
{
    public class ApplicationErrorOccured : PubSubEvent<ApplicationErrorOccuredArgs>
    {
    }

    public class ApplicationErrorOccuredArgs
    {
        public string Error { get; set; }
    }
}
