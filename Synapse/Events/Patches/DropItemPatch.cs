using System;
using Harmony;
using Synapse.Api;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(Inventory), nameof(Inventory.CallCmdDropItem))]
    public class DropItemPatch
    {
        public static bool Prefix(Inventory __instance, int itemInventoryIndex)
        {
            try
            {
                if (!__instance._iawRateLimit.CanExecute() || itemInventoryIndex < 0 ||
                    itemInventoryIndex >= __instance.items.Count) return false;

                var syncItemInfo = __instance.items[itemInventoryIndex];

                if (__instance.items[itemInventoryIndex].id != syncItemInfo.id) return false;

                var allow = true;

                Events.InvokeDropItem(__instance.GetPlayer(), ref syncItemInfo, ref allow);

                if (!allow) return false;

                var dropped = __instance.SetPickup(syncItemInfo.id, syncItemInfo.durability,
                    __instance.transform.position, __instance.camera.transform.rotation, syncItemInfo.modSight,
                    syncItemInfo.modBarrel, syncItemInfo.modOther);

                __instance.items.RemoveAt(itemInventoryIndex);

                //TODO: InvokeItemDroped

                return false;
            }
            catch (Exception e)
            {
                Log.Error($"DropItemPatch Err: {e}");
                return true;
            }
        }
    }
}