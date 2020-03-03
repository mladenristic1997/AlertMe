using System.Collections.Generic;

namespace AlertMe.Domain
{
    public class StoredAlertPlans
    {
        public List<string> AlertPlans { get; set; }

        public StoredAlertPlans()
        {
            AlertPlans = new List<string>();
        }
    }
}
