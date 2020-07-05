using System;
using Harmony;
using Synapse.Api;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(ReferenceHub), nameof(ReferenceHub.OnDestroy))]
    public static class PlayerLeavePatch
    {
        // ReSharper disable once InconsistentNaming
        public static bool Prefix(ReferenceHub __instance)
        {
            try
            {
                Events.InvokePlayerLeaveEvent(__instance.GetPlayer());
            }
            catch (Exception e)
            {
                Log.Error($"Player Leave Event Error: {e}");
            }

            return true;
        }
    }
}