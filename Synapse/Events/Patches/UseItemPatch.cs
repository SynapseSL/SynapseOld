using System;
using Harmony;
using Synapse.Api;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(ConsumableAndWearableItems), nameof(ConsumableAndWearableItems.UseMedicalItem))]
    public class UseItemPatch
    {
        public static bool Prefix(ConsumableAndWearableItems __instance)
        {
            try
            {
                Events.InvokeUseItemEvent(__instance.GetPlayer(),out bool allow);
                return allow;
            }
            catch (Exception e)
            {
                Log.Error($"UseItemEvent Error: {e}");
                return true;
            }
        }
    }
}
