using System.Collections.Generic;

namespace AlertMe.Domain
{
    public class AlertConfig
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Alert> Alerts { get; set; }

        public AlertConfig()
        {
            Alerts = new List<Alert>();
        }
    }
}
