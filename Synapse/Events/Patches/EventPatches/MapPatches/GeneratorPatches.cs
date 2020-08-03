using Harmony;
using Synapse.Api;
using UnityEngine;

namespace Synapse.Events.Patches
{
	/*
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
					Log.Error("GeneratorEventError: a GameObject which is not a Player used a Generator?");
					return false;
                }

				if (command.StartsWith("EPS_DOOR")) return true;

				if (command.StartsWith("EPS_TABLET"))
				{
					if (__instance.isTabletConnected || !__instance.isDoorOpen || __instance._localTime <= 0f || Generator079.mainGenerator.forcedOvercharge) return false;

					var component = person.GetComponent<Inventory>();
					var enumerator = component.items.GetEnumerator();
					while (enumerator.MoveNext())
					{
						var syncItemInfo = enumerator.Current;
						if (syncItemInfo.id != ItemType.WeaponManagerTablet) continue;
						var allow = player.Team != Team.SCP;
						Events.InvokeGeneratorInserted(player, __instance, ref allow);
						if (!allow) return false;
						component.items.Remove(syncItemInfo);
						__instance.NetworkisTabletConnected = true;
						return false;
					}
				}

				if (!command.StartsWith("EPS_CANCEL")) return true;
				{
					if (!__instance.isTabletConnected) return false;
					var allow = true;

					Events.InvokeGeneratorEjected(player, __instance, ref allow);
					return allow;
				}
            }
			catch (Exception e)
            {
				Log.Error($"GeneratorTablet Event Error: {e}");
				return true;
            }
        }
    }*/

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
