using System;
using Harmony;
using Mirror;
using Synapse.Api;
using UnityEngine;

namespace Synapse.Events.Patches.EventPatches.ScpPatches
{
    [HarmonyPatch(typeof(PocketDimensionTeleport), nameof(PocketDimensionTeleport.OnTriggerEnter))]
    internal static class PocketDimensionLeavePatch
    {
        private static bool Prefix(PocketDimensionTeleport __instance, Collider other)
        {
            try
            {
                var component = other.GetComponent<NetworkIdentity>();
                if (!NetworkServer.active || component == null) return false;

                Events.InvokePocketDimensionLeave(component.GetPlayer(), ref __instance.type, out var allow);

                return allow;
            }
            catch (Exception e)
            {
                Log.Error($"PocketDimExit Event Error: {e}");
                return true;
            }
        }
    }
}
