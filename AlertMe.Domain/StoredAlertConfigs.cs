using System.Collections.Generic;

namespace AlertMe.Domain
{
    public class StoredAlertConfigs
    {
        public List<string> AlertConfigs { get; set; }

        public StoredAlertConfigs()
        {
            AlertConfigs = new List<string>();
        }
    }
}
