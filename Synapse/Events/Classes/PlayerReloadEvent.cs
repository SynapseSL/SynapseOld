using Synapse.Api;

namespace Synapse.Events.Classes
{
    public class PlayerReloadEvent
    {
        public Player Player { get; internal set; }

        public bool Allow { get; set; }

        public Inventory.SyncItemInfo InventorySlot { get; internal set; }

        public WeaponManager.Weapon Weapon { get; set; }
    }
}
