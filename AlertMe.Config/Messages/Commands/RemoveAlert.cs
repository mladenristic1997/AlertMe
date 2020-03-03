﻿using Prism.Events;

namespace AlertMe.Plans.Commands
{
    public class RemoveAlert : PubSubEvent<RemoveAlertArgs>
    {
    }

    public class RemoveAlertArgs
    {
        public string Id { get; set; }
    }
}
