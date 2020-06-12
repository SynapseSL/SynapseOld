using System;
using Harmony;
using Synapse.Api;
using UnityEngine;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(PlayerStats), nameof(PlayerStats.HurtPlayer))]
    public class PlayerDamagePatch
    {
        public static void Prefix(PlayerStats __instance, ref PlayerStats.HitInfo info, GameObject go)
        {
            try
            {
                if (go == null) return;

                var killer = __instance.GetComponent<ReferenceHub>();

                if (info.GetDamageType() == DamageTypes.Grenade)
                    killer = PlayerExtensions.GetPlayer(info.PlayerId).Hub;

                var player = go.GetComponent<ReferenceHub>();

                Events.InvokePlayerHurtEvent(player, killer, ref info);
            }
            catch (Exception e)
            {
                Log.Error($"PlayerDamageEvent Error: {e}");
            }
        }

        public static void Postfix(PlayerStats __instance, PlayerStats.HitInfo info, GameObject go)
        {
            try
            {
                var killer = __instance.GetComponent<ReferenceHub>();
                var player = go.GetComponent<ReferenceHub>();

                if (player.GetPlayer().Role == RoleType.Spectator)
                    Events.InvokePlayerDieEvent(player, killer, info);
            }
            catch (Exception e)
            {
                Log.Error($"PlayerDie Event Error: {e}");
            }
        }
    }
}