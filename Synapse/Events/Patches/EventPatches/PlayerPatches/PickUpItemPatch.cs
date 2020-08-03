using System;
using Harmony;
using Mirror;
using Searching;
using Synapse.Api;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(SearchCoordinator), nameof(SearchCoordinator.ContinuePickupServer))]
    static class PickUpItemPatch
    {
        public static bool Prefix(SearchCoordinator __instance)
        {
            try
            {
                if (__instance.Completor.ValidateUpdate())
                {
                    if (NetworkTime.time >= __instance.SessionPipe.Session.FinishTime)
                    {
                        bool allow = true;

                        Events.InvokePickupItemEvent(__instance.GetPlayer(),__instance.Completor.TargetPickup, ref allow);

                        if (allow)
                        {
                            __instance.Completor.Complete();
                            return false;
                        }
                    }
                }
                else
                {
                    __instance.SessionPipe.Invalidate();
                }

                return false;
            }
            catch (Exception e)
            {
                Log.Error($"PickupItem Event Error: {e}");
                return false;
            }
        }
    }
}
