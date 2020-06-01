﻿using Harmony;
using UnityEngine;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(PlayerInteract), nameof(PlayerInteract.CallCmdOpenDoor))]
    public class DoorInteractPatch
    {
        public static bool Prefix(PlayerInteract __instance, GameObject doorId)
        {
            var allowed = true;
            var door = doorId.GetComponent<Door>();
            var player = __instance.gameObject.GetComponent<ReferenceHub>();
            
            Events.InvokeDoorInteraction(player, door, ref allowed);

            return allowed;
        }
    }
}