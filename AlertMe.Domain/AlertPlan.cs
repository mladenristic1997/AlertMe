using System.Collections.Generic;

namespace AlertMe.Domain
{
    public class AlertPlan
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Alert> Alerts { get; set; }

        public AlertPlan()
        {
            Alerts = new List<Alert>();
        }
    }
}
