using System;

namespace Synapse.Events
{
    public class PlayerJoinEvent : EventArgs
    {
        public ReferenceHub Player { get; set; }
    }
}
