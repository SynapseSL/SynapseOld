using System;
using Harmony;
using Mirror;
using Synapse.Api;
using UnityEngine;

namespace Synapse.Events.Patches
{
	[HarmonyPatch(typeof(Generator079), nameof(Generator079.Interact))]
	public static class GeneratorTabletPatches
	{
		public static bool Prefix(Generator079 __instance, GameObject person, PlayerInteract.Generator079Operations command)
		{
			try
			{
				switch (command)
				{
					case PlayerInteract.Generator079Operations.Door:



						break;

					case PlayerInteract.Generator079Operations.Tablet:

						if (__instance.isTabletConnected || !__instance.isDoorOpen || __instance._localTime <= 0f || Generator079.mainGenerator.forcedOvercharge)
						{
							return false;
						}
						Inventory component = person.GetComponent<Inventory>();
						using (SyncList<Inventory.SyncItemInfo>.SyncListEnumerator enumerator = component.items.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Inventory.SyncItemInfo syncItemInfo = enumerator.Current;
								if (syncItemInfo.id == ItemType.WeaponManagerTablet)
								{
									var allow2 = true;
									Events.InvokeGeneratorInserted(person.GetPlayer(), __instance, ref allow2);
									if (!allow2) break;

									component.items.Remove(syncItemInfo);
									__instance.NetworkisTabletConnected = true;
									break;
								}
							}
						}
						break;

					case PlayerInteract.Generator079Operations.Cancel:
						if (!__instance.isTabletConnected) break;

						var allow = true;
						Events.InvokeGeneratorEjected(person.GetPlayer(), __instance, ref allow);
						if (!allow) break;
						return true;
				}
				return false;
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

			var component = person.GetComponent<Inventory>();
			if (component == null || __instance._doorAnimationCooldown > 0f || __instance._deniedCooldown > 0f) return false;

			//Check if the Generator can be open or must be unlocked
			if (__instance.isDoorUnlocked)
			{
				var allow = true;
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

				__instance._doorAnimationCooldown = 1.5f;
				__instance.NetworkisDoorOpen = !__instance.isDoorOpen;
				__instance.RpcDoSound(__instance.isDoorOpen);
				return false;
			}

			//Unlock The Generator
			var flag = player.Bypass;
			var flag2 = player.Team != Team.SCP;

			if (flag2 && component.curItem > ItemType.KeycardJanitor)
			{
				var permissions = component.GetItemByID(component.curItem).permissions;

				foreach (var t in permissions)
					if (t == "ARMORY_LVL_2")
						flag = true;
			}

			Events.InvokeGeneratorUnlock(player, __instance, ref flag);

			if (flag)
			{
				__instance.NetworkisDoorUnlocked = true;
				__instance._doorAnimationCooldown = 0.5f;
				return false;
			}
			__instance.RpcDenied();

			return false;
		}
	}
}
