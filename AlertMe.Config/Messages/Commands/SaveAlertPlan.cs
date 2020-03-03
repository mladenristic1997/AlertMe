using AlertMe.Domain;
using Prism.Events;

namespace AlertMe.Plans.Commands
{
    public class SaveAlertPlan : PubSubEvent<AlertPlan>
    {
    }
}
