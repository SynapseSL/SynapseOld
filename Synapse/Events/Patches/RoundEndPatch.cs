using System;
using Harmony;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(RoundSummary), nameof(RoundSummary._ProcessServerSideCode))]
    public class RoundEndPatch
    {
        public static void Postfix(RoundSummary __instance)
        {
            try
            {
                if (__instance.roundEnded)
                    Events.InvokeRoundEndEvent();
            }
            catch (Exception e)
            {
                Log.Error($"RoundEnd Event Error: {e}");
            }
        }
    }
}
