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
            if (__instance.GetComponent<Player>() == null) __instance.gameObject.AddComponent<Player>();

            if (__instance.GetComponent<Jail>() == null) __instance.gameObject.AddComponent<Jail>();

            if (__instance.GetComponent<Scp106Controller>() == null) __instance.gameObject.AddComponent<Scp106Controller>();

            try
            {
                Events.InvokeLoadComponents(__instance.gameObject);
            }
            catch (Exception e)
            {
                Log.Error($"LoadComponentsEvent Error: {e}");
            }
        }
    }
}
