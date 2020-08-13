using System;
using Grenades;
using Harmony;
using Mirror;
using Synapse.Api;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(GrenadeManager), nameof(GrenadeManager.CallCmdThrowGrenade))]
    public class PlayerThrowGrenadePatch
    {
        public static bool Prefix(ref GrenadeManager __instance, ref int id, ref bool slowThrow, ref double time)
        {
            try
            {
                var player = __instance.GetPlayer();
                if (player == null) return true;
                ItemType item;
                switch (id)
                {
                    case 0:
                        item = ItemType.GrenadeFlash;
                        break;
                    case 1:
                        item = ItemType.GrenadeFrag;
                        break;
                    case 2:
                        item = ItemType.SCP018;
                        break;
                    default:
                        Log.Error($"Invalid Grenade ID: {id}");
                        return true;
                }
                
                time -= NetworkTime.time;

                Events.InvokeUseItemEvent(player, item, out var useAllow);

                if (!useAllow) return false;

                Events.InvokePlayerThrowGrenadeEvent(player, item, ref slowThrow, ref time, out var allow);
                
                
                time += NetworkTime.time;
                
                
                return allow;

            }
            catch (Exception e)
            {
                Log.Error($"Player Throw Grenade Error: {e}");
                return true;
            }
        }
    }
}