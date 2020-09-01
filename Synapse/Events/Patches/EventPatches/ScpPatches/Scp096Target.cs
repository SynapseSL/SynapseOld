using System;
using Harmony;
using PlayableScps;
using Synapse.Api;

namespace Synapse.Events.Patches.EventPatches.ScpPatches
{
    [HarmonyPatch(typeof(Scp096), nameof(Scp096.ParseVisionInformation))]
    static class Scp096LookPatch
    {
        public static bool Prefix(Scp096 __instance ,VisionInformation info)
        {
            try
            {
                var allow = true;
                Events.InvokeScp096AddTarget(info.Source.GetPlayer(), __instance.GetPlayer(), __instance.PlayerState, ref allow);
                return allow;
            }
            catch(Exception e)
            {
                Log.Info($"Scp096AddTarget Event Error: {e}");
                return true;
            }
        }
    }

    [HarmonyPatch(typeof(Scp096), nameof(Scp096.OnDamage))]
    static class Scp096ShootPatch
    {
        public static bool Prefix(Scp096 __instance, PlayerStats.HitInfo info)
        {
            try
            {
                if (info.RHub == null)
                    return true;

                var allow = true;
                Events.InvokeScp096AddTarget(info.RHub.GetPlayer(), __instance.GetPlayer(), __instance.PlayerState, ref allow);
                return allow;
            }
            catch (Exception e)
            {
                Log.Info($"Scp096AddTarget Event Error: {e}");
                return true;
            }
        }
    }
}
