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

                var killer = __instance.GetPlayer();

                if (info.GetDamageType() == DamageTypes.Grenade)
                    killer = Player.GetPlayer(info.PlayerId);

                var player = go.GetPlayer();

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
                var killer = __instance.GetPlayer();
                var player = go.GetPlayer();

                if (player.Role == RoleType.Spectator)
                    Events.InvokePlayerDieEvent(player, killer, info);
            }
            catch (Exception e)
            {
                Log.Error($"PlayerDie Event Error: {e}");
            }
        }
    }
}