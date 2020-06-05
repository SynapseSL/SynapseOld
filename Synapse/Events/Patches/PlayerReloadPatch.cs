using System;
using Harmony;

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
                var itemIndex = __instance.hub.inventory.GetItemIndex();
                var weapon = __instance.weapons[__instance._reloadingWeapon];
                var inventoryslot = __instance.hub.inventory.items[itemIndex];
                var player = __instance.hub;

                if (itemIndex < 0 || itemIndex >= __instance.hub.inventory.items.Count) return false;
                if (__instance.curWeapon < 0 || __instance.hub.inventory.curItem != __instance.weapons[__instance.curWeapon].inventoryID) return false;
                if (__instance.hub.inventory.items[itemIndex].durability >= __instance.weapons[__instance.curWeapon].maxAmmo) return false;

                Events.InvokePlayerReloadEvent(player, ref allow, ref weapon, inventoryslot);

                __instance.weapons[__instance._reloadingWeapon] = weapon;

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
