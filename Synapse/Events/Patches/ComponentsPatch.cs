using System;
using Harmony;
using Synapse.Api;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(ReferenceHub), nameof(ReferenceHub.LoadComponents))]
    public static class ComponentsPatch
    {
        public static void Prefix(ReferenceHub __instance)
        {
            if (__instance.GetComponent<Player>() == null)
                __instance.gameObject.AddComponent<Player>();

            Events.InvokeLoadComponents(__instance.GetPlayer());
        }
    }
}
