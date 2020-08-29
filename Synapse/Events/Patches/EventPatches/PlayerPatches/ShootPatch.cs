using System;
using Harmony;
using Synapse.Api;
using UnityEngine;

namespace Synapse.Events.Patches.EventPatches.PlayerPatches
{
    [HarmonyPatch(typeof(WeaponManager), nameof(WeaponManager.CallCmdShoot))]
    static class ShootPatch
    {
        private static bool Prefix(WeaponManager __instance, GameObject target, string hitboxType, Vector3 dir, Vector3 sourcePos, Vector3 targetPos)
        {
            try
            {
                if (!__instance._iawRateLimit.CanExecute(true))
                    return false;
                int itemIndex = __instance._hub.inventory.GetItemIndex();
                if (itemIndex < 0 || itemIndex >= __instance._hub.inventory.items.Count || __instance.curWeapon < 0 ||
                    ((__instance._reloadCooldown > 0.0 || __instance._fireCooldown > 0.0) &&
                     !__instance.isLocalPlayer) ||
                    (__instance._hub.inventory.curItem != __instance.weapons[__instance.curWeapon].inventoryID ||
                     __instance._hub.inventory.items[itemIndex].durability <= 0.0))
                    return false;

                Player targetplayer = null;
                if (target != null)
                    targetplayer = target.GetPlayer();

                //Event Invoke
                Events.InvokeShootEvent(__instance.gameObject.GetPlayer(), targetplayer, ref targetPos, out var allow);

                return allow;
            }
            catch (Exception e)
            {
                Log.Error($"Shoot Event Error: {e}");

                return true;
            }
        }
    }
}
