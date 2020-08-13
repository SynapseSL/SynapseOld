﻿using System;
 using Harmony;
using Synapse;
 using Synapse.Api;

 namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(PlayerStats), nameof(PlayerStats.HealHPAmount))]
    public class PlayerHealPatch
    {
        public static bool Prefix(PlayerStats __instance, ref float hp)
        {
            try
            {
                var player = __instance.GetPlayer();
            
                Events.InvokePlayerHealEvent(player, ref hp, out var allow);
            
                return allow;
            }
            catch (Exception e)
            {
                Log.Info($"Player Heal Event Error: {e}");
                return true;
            }
        }
    }
}