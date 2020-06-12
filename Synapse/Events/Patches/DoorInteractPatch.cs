﻿using System;
using Harmony;
using Synapse.Api;
using UnityEngine;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(PlayerInteract), nameof(PlayerInteract.CallCmdOpenDoor))]
    public class DoorInteractPatch
    {
        public static bool Prefix(PlayerInteract __instance, GameObject doorId)
        {
            try
            {
                var allowed = true;
                var door = doorId.GetComponent<Door>();
                var player = __instance.GetPlayer();

                Events.InvokeDoorInteraction(player, door, ref allowed);

                return allowed;
            }
            catch (Exception e)
            {
                Log.Error($"DoorInteraction Error: {e}");
                return true;
            }
        }
    }
}