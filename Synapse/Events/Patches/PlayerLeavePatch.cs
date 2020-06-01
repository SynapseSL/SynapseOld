using System;

namespace Synapse.Events.Patches
{
    public static class PlayerLeavePatch
    {
        // ReSharper disable once InconsistentNaming
        public static bool Prefix(ReferenceHub __instance)
        {
            try
            {
                Events.InvokePlayerLeaveEvent(__instance);
            }
            catch (Exception e)
            {
                Log.Error($"Player Leave Event Error: {e}");
            }

            return true;
        }
    }
}
