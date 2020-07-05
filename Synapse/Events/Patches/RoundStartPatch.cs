using System;
using Harmony;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.CmdStartRound))]
    public class RoundStartPatch
    {
        public static void Prefix()
        {
            try
            {
                Events.InvokeRoundStart();
            }
            catch (Exception exception)
            {
                Log.Error($"RoundStartEvent Err: {exception}");
            }
        }
    }
}