namespace Synapse.Events.Classes
{
    public class DropItemEvent
    {
        public ReferenceHub Player { get; internal set; }
        public Inventory.SyncItemInfo Item { get; set; }
        public bool Allow { get; set; }
    }
}