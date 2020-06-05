namespace Synapse.Events.Classes
{
    public class PlayerReloadEvent
    {
        public ReferenceHub Player { get; internal set; }

        public bool Allow { get; set; }

        public Inventory.SyncItemInfo InventorySlot { get; internal set; }

        public WeaponManager.Weapon Weapon { get; set; }
    }
}
