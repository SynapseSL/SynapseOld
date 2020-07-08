using System;
using Harmony;
using Synapse.Api;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(ServerRoles), nameof(ServerRoles.SetGroup))]
    public static class PlayerSetGroupPatch
    {
        public static bool Prefix(ServerRoles __instance, ref UserGroup group, ref bool ovr, bool byAdmin, ref bool disp)
        {
            try
            {
                Events.InvokePlayerSetGroupEvent(__instance.GetPlayer(), byAdmin, ref group, ref ovr, ref disp, out bool allow);

                if (__instance._globalPerms > 0UL) return true;

                return allow;
            }
            catch(Exception e)
            {
                Log.Error($"PlayerSetGroup Event Error: {e}");
                return true;
            }
        }
    }
}
