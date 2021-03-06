﻿using System;
using Harmony;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(PlayerStats), nameof(PlayerStats.Roundrestart))]
    public class RoundRestartPatch
    {
        public static void Prefix()
        {
            try
            {
                Events.InvokeRoundRestart();
            }
            catch (Exception e)
            {
                Log.Error($"RoundRestart Error: {e}");
            }
        }
    }
}