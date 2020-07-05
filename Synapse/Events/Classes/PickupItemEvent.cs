using Synapse.Api;

namespace Synapse.Events.Classes
{
    public class PickupItemEvent
    {
        public Player Player { get; internal set; }

        public Pickup Pickup { get; internal set; }

        public bool Allow { get; set; }
    }
}
