namespace Synapse.Events.Classes
{
    public class PlayerReloadClass
    {
        public ReferenceHub Player { get; internal set; }

        public bool Allow { get; internal set; }

        public Inventory.SyncItemInfo InventorySlot { get; internal set; }

        public WeaponManager.Weapon Weapon { get; internal set; }
    }
}
