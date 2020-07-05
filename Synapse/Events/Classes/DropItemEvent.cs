using Synapse.Api;

namespace Synapse.Events.Classes
{
    public class DropItemEvent
    {
        public Player Player { get; internal set; }
        public Inventory.SyncItemInfo Item { get; set; }
        public bool Allow { get; set; }
    }
}