using System;
using Harmony;
using UnityEngine;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(Handcuffs), nameof(Handcuffs.CallCmdCuffTarget))]
    public class PlayerCuffPatch
    {
        public static bool Prefix(Handcuffs __instance, GameObject target)
        {
            try
            {
				if (!__instance._interactRateLimit.CanExecute(true))
				{
					return false;
				}
				if (target == null || Vector3.Distance(target.transform.position, __instance.transform.position) > __instance.raycastDistance * 1.1f)
				{
					return false;
				}
				global::Handcuffs handcuffs = global::ReferenceHub.GetHub(target).handcuffs;
				if (handcuffs == null || __instance.MyReferenceHub.inventory.curItem != global::ItemType.Disarmer || __instance.MyReferenceHub.characterClassManager.CurClass < global::RoleType.Scp173)
				{
					return false;
				}
				if (handcuffs.CufferId < 0 && handcuffs.MyReferenceHub.inventory.curItem == global::ItemType.None)
				{
					global::Team team = __instance.MyReferenceHub.characterClassManager.CurRole.team;
					global::Team team2 = handcuffs.MyReferenceHub.characterClassManager.CurRole.team;
					bool flag = false;
					if (team == global::Team.CDP)
					{
						if (team2 == global::Team.MTF || team2 == global::Team.RSC)
						{
							flag = true;
						}
					}
					else if (team == global::Team.RSC)
					{
						if (team2 == global::Team.CHI || team2 == global::Team.CDP)
						{
							flag = true;
						}
					}
					else if (team == global::Team.CHI)
					{
						if (team2 == global::Team.MTF || team2 == global::Team.RSC)
						{
							flag = true;
						}
						if (team2 == global::Team.CDP && GameCore.ConfigFile.ServerConfig.GetBool("ci_can_cuff_class_d", false))
						{
							flag = true;
						}
					}
					else if (team == global::Team.MTF)
					{
						if (team2 == global::Team.CHI || team2 == global::Team.CDP)
						{
							flag = true;
						}
						if (team2 == global::Team.RSC && GameCore.ConfigFile.ServerConfig.GetBool("mtf_can_cuff_researchers", false))
						{
							flag = true;
						}
					}
					if (flag)
					{
						__instance.ClearTarget();
						handcuffs.NetworkCufferId = __instance.MyReferenceHub.queryProcessor.PlayerId;
					}
				}

				return false;
            }
            catch (Exception e)
            {
                Log.Error($"PlayerCuffedEvent Error: {e}");
                return true;
            }
        }
    }
}
