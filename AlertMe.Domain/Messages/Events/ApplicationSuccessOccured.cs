using Prism.Events;

namespace AlertMe.Domain.Events
{
    public class ApplicationSuccessOccured : PubSubEvent<ApplicationSuccessOccuredArgs>
    {
    }

    public class ApplicationSuccessOccuredArgs
    {
        public string Message { get; set; }
    }
}
