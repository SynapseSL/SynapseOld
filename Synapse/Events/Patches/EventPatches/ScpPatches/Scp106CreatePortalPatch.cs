using Harmony;
using Synapse.Api;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(Scp106PlayerScript), nameof(Scp106PlayerScript.CallCmdMakePortal))]
    public class Scp106CreatePortalPatch
    {
        private static bool Prefix(Scp106PlayerScript __instance)
        {
            var allow = true;
            Events.InvokeScp106CreatePortalEvent(__instance.gameObject.GetPlayer(), ref allow);
            return allow;
        }
    }
}