using System;
using Harmony;
using Synapse.Api;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.CallCmdRequestShowTag))]
    public static class ShowTag
    {
        public static bool Prefix(CharacterClassManager __instance)
        {
            try
            {
                Events.InvokePlayerTagEvent(__instance.GetPlayer(), true, out bool allow);
                return allow;
            }
            catch (Exception e)
            {
                Log.Error($"PlayerTagEvent Error: {e}");
                return true;
            }
        }
    }

    [HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.CallCmdRequestHideTag))]
    public static class HideTag
    {
        public static bool Prefix(CharacterClassManager __instance)
        {
            try
            {
                Events.InvokePlayerTagEvent(__instance.GetPlayer(), false, out bool allow);
                return allow;
            }
            catch(Exception e)
            {
                Log.Error($"PlayerTagEvent Error: {e}");
                return true;
            }
        }
    }
}
