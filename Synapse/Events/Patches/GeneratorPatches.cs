using System;
using Harmony;
using Synapse.Api;
using UnityEngine;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(Generator079), nameof(Generator079.Interact))]
    public static class GeneratorTabletPatches
    {
        public static bool Prefix(Generator079 __instance, GameObject person, string command)
        {
            try
            {
				var player = person.GetPlayer();

				if (player == null)
                {
					Log.Error("GeneratorEventError: a gameobject which is not a Player used a Generator?");
					return false;
                }

				if (command.StartsWith("EPS_DOOR")) return true;

				if (command.StartsWith("EPS_TABLET"))
				{
					if (__instance.isTabletConnected || !__instance.isDoorOpen || __instance.localTime <= 0f || Generator079.mainGenerator.forcedOvercharge) return false;

					var component = person.GetComponent<Inventory>();
					var enumerator = component.items.GetEnumerator();
					while (enumerator.MoveNext())
					{
						Inventory.SyncItemInfo syncItemInfo = enumerator.Current;
						if (syncItemInfo.id == ItemType.WeaponManagerTablet)
						{
							bool allow = player.Team != Team.SCP;
							Events.InvokeGeneratorInserted(player, __instance, ref allow);
							if (allow)
							{
								component.items.Remove(syncItemInfo);
								__instance.NetworkisTabletConnected = true;
							}
							return false;
						}
					}
				}

				if (command.StartsWith("EPS_CANCEL"))
				{
					if (!__instance.isTabletConnected) return false;
					bool allow = true;

					Events.InvokeGeneratorEjected(player, __instance, ref allow);
					return allow;
				}
				return true;
			}
			catch (Exception e)
            {
				Log.Error($"GeneratorTablet Event Error: {e}");
				return true;
            }
        }
    }

    [HarmonyPatch(typeof(Generator079), nameof(Generator079.OpenClose))]
    public static class GeneratorDoorPatches
    {
        public static bool Prefix(Generator079 __instance, GameObject person)
        {
			var player = person.GetPlayer();

			Inventory component = person.GetComponent<Inventory>();
			if (component == null || __instance.doorAnimationCooldown > 0f || __instance.deniedCooldown > 0f) return false;

			//Check if the Generator can be open or must be unlocked
			if (__instance.isDoorUnlocked)
			{
				bool allow = true;
				if (!__instance.NetworkisDoorOpen)
                {
					Events.InvokeGeneratorOpen(player, __instance, ref allow);
                }
                else
                {
					Events.InvokeGeneratorClose(player, __instance, ref allow);
                }

				if (!allow)
                {
					__instance.RpcDenied();
					return false;
                }

				__instance.doorAnimationCooldown = 1.5f;
				__instance.NetworkisDoorOpen = !__instance.isDoorOpen;
				__instance.RpcDoSound(__instance.isDoorOpen);
				return false;
			}

			//Unlock The Generator
			bool flag = player.Bypass;
			bool flag2 = player.Team != Team.SCP;
			
			if (flag2 && component.curItem > ItemType.KeycardJanitor)
			{
				string[] permissions = component.GetItemByID(component.curItem).permissions;

				for (int i = 0; i < permissions.Length; i++)
					if (permissions[i] == "ARMORY_LVL_2")
						flag = true;
			}

			Events.InvokeGeneratorUnlock(player, __instance, ref flag);

			if (flag)
			{
				__instance.NetworkisDoorUnlocked = true;
				__instance.doorAnimationCooldown = 0.5f;
				return false;
			}
			__instance.RpcDenied();

			return false;
		}
    }
}
