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

				//Get The Handcuff of the Target
				Handcuffs handcuffs = ReferenceHub.GetHub(target).handcuffs;
				if (handcuffs == null || __instance.MyReferenceHub.inventory.curItem != ItemType.Disarmer || __instance.MyReferenceHub.characterClassManager.CurClass < RoleType.Scp173)
				{
					return false;
				}

				if (handcuffs.CufferId < 0 && handcuffs.MyReferenceHub.inventory.curItem == ItemType.None)
				{
					//Team of the person who cuffs someone
					Team team = __instance.MyReferenceHub.characterClassManager.CurRole.team;
					//Team of the Person who will becom cuffed
					Team team2 = handcuffs.MyReferenceHub.characterClassManager.CurRole.team;

					bool flag = false;

					//Check for When the Cuffer is a DBoy
					if (team == Team.CDP)
					{
						if (team2 == Team.MTF || team2 == Team.RSC)
						{
							flag = true;
						}
					}

					//Check for when the Cuffer is a Nerd
					else if (team == Team.RSC)
					{
						if (team2 == Team.CHI || team2 == Team.CDP)
						{
							flag = true;
						}
					}

					//Check for when the Cuffer is a Chaos
					else if (team == Team.CHI)
					{
						if (team2 == Team.MTF || team2 == Team.RSC)
						{
							flag = true;
						}
						if (team2 == Team.CDP && GameCore.ConfigFile.ServerConfig.GetBool("ci_can_cuff_class_d", false))
						{
							flag = true;
						}
					}

					//Check for when the Cuffer is a Mtf
					else if (team == Team.MTF)
					{
						if (team2 == Team.CHI || team2 == Team.CDP)
						{
							flag = true;
						}
						if (team2 == global::Team.RSC && GameCore.ConfigFile.ServerConfig.GetBool("mtf_can_cuff_researchers", false))
						{
							flag = true;
						}
					}

					//Event
					ReferenceHub cuffer = __instance.MyReferenceHub;
					ReferenceHub target2 = handcuffs.MyReferenceHub;
					Events.InvokePlayerCuffedEvent(cuffer, target2, ref flag);

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
