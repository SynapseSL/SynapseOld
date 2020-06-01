using System;
using Harmony;
using Synapse.Api;
using UnityEngine;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(PlayerStats), nameof(PlayerStats.HurtPlayer))]
    public class PlayerDamagePatch
    {
        public static bool Prefix(PlayerStats __instance,PlayerStats.HitInfo info, GameObject go)
        {
            try
            {
                return true;
            }
            catch(Exception e)
            {
                Log.Error($"PlayerDamageEvent Error: {e}");
                return true;
            }
        }

        public static void Postfix(PlayerStats __instance, PlayerStats.HitInfo info, GameObject go)
        {
            try
            {
                var player = __instance.GetComponent<ReferenceHub>();
                var killer = go.GetComponent<ReferenceHub>();

                if (player.GetRole() == RoleType.Spectator)
                    Events.InvokePlayerDieEvent(player, killer, info);
            }
            catch(Exception e)
            {
                Log.Error($"PlayerDie Event Error: {e}");
            }
        }
    }
}
