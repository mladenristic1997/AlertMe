using AlertMe.Domain;
using Prism.Events;

namespace AlertMe.Config.Commands
{
    public class SaveConfig : PubSubEvent<AlertConfig>
    {
    }
}
