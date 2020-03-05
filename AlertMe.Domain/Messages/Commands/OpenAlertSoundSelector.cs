using Prism.Events;

namespace AlertMe.Domain.Commands
{
    public class OpenAlertSoundSelector : PubSubEvent<OpenAlertSoundSelectorArgs>
    {
    }

    public class OpenAlertSoundSelectorArgs
    {
        public string PlanId { get; set; }
        public string AlertId { get; set; }
    }
}
