using System;
using System.Collections.Generic;
using Harmony;
using Synapse.Api;
using UnityEngine;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(TeslaGate), nameof(TeslaGate.PlayersInRange))]
    public static class TeslaTriggerEvent
    {
        public static void Postfix(TeslaGate __instance, bool hurtRange, ref List<PlayerStats> __result)
        {
            try
            {
                __result = new List<PlayerStats>();

                foreach(var player in Player.GetAllPlayers())
                {
                    var triggerable = true;

                    if (!(Vector3.Distance(__instance.transform.position, player.Position) <
                          __instance.sizeOfTrigger) || player.Role == RoleType.Spectator) continue;
                    Events.InvokeTeslaTrigger(player, hurtRange, ref triggerable);

                    if (triggerable) __result.Add(player.GetComponent<PlayerStats>());
                }
            }
            catch(Exception e)
            {
                Log.Error($"TriggerTeslaEvent error: {e}");
            }
        }
    }
}
