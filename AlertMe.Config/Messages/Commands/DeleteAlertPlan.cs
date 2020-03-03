using Prism.Events;

namespace AlertMe.Plans.Commands
{
    public class DeleteAlertPlan : PubSubEvent<DeleteAlertPlanArgs>
    {
    }

    public class DeleteAlertPlanArgs
    {
        public string Id { get; set; }
    }
}
