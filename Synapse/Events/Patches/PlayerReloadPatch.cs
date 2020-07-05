using System;
using Harmony;
using Synapse.Api;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(WeaponManager), nameof(WeaponManager.CallCmdReload))]
    public class PlayerReloadPatch
    {
        public static bool Prefix(WeaponManager __instance)
        {
            try
            {
                if (!__instance._iawRateLimit.CanExecute(true)) return false;

                var allow = true;
                var itemIndex = __instance._hub.inventory.GetItemIndex();
                var inventoryslot = __instance._hub.inventory.items[itemIndex];
                var player = __instance._hub.GetPlayer();

                if (itemIndex < 0 || itemIndex >= __instance._hub.inventory.items.Count) return false;
                if (__instance.curWeapon < 0 || __instance._hub.inventory.curItem != __instance.weapons[__instance.curWeapon].inventoryID) return false;
                if (__instance._hub.inventory.items[itemIndex].durability >= __instance.weapons[__instance.curWeapon].maxAmmo) return false;

                Events.InvokePlayerReloadEvent(player, ref allow, inventoryslot);

                return allow;
            }
            catch (Exception e)
            {
                Log.Error($"PlayerReloadEvent Error: {e}");
                return true;
            }
        }
    }
}
