using System;
using Harmony;
using Synapse.Api;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(Scp079PlayerScript), nameof(Scp079PlayerScript.TargetLevelChanged))]
    public static class Scp079GainLvlPatch
    {
        public static bool Prefix(Scp079PlayerScript __instance, ref int newLvl)
        {
            try
            {
                bool allow = true;
                Events.InvokeScp079LvlEvent(__instance.GetPlayer(), ref newLvl, ref allow);
                return allow;
            }
            catch (Exception e)
            {
                Log.Error($"Scp079GainLevelEvent Error: {e}");
                return true;
            }
        }
    }
}
