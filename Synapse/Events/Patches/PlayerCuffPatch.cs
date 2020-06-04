using System;
using GameCore;
using Harmony;
using UnityEngine;

// ReSharper disable SwitchStatementMissingSomeEnumCasesNoDefault

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(Handcuffs), nameof(Handcuffs.CallCmdCuffTarget))]
    public class PlayerCuffPatch
    {
        public static bool Prefix(Handcuffs __instance, GameObject target)
        {
            try
            {
                if (!__instance._interactRateLimit.CanExecute()) return false;

                if (target == null || Vector3.Distance(target.transform.position, __instance.transform.position) >
                    __instance.raycastDistance * 1.1f) return false;

                //Get The Handcuff of the Target
                var handcuffs = ReferenceHub.GetHub(target).handcuffs;
                if (handcuffs == null || __instance.MyReferenceHub.inventory.curItem != ItemType.Disarmer ||
                    __instance.MyReferenceHub.characterClassManager.CurClass < RoleType.Scp173) return false;

                if (handcuffs.CufferId >= 0 || handcuffs.MyReferenceHub.inventory.curItem != ItemType.None)
                    return false;
                //Team of the person who cuffs someone
                var team = __instance.MyReferenceHub.characterClassManager.CurRole.team;
                //Team of the Person who will become cuffed
                var team2 = handcuffs.MyReferenceHub.characterClassManager.CurRole.team;

                var flag = false;

                switch (team)
                {
                    //Check for When the Cuffer is a DBoy
                    case Team.CDP:
                    {
                        if (team2 == Team.MTF || team2 == Team.RSC) flag = true;
                        break;
                    }
                    //Check for when the Cuffer is a Nerd
                    case Team.RSC:
                    {
                        if (team2 == Team.CHI || team2 == Team.CDP) flag = true;
                        break;
                    }
                    //Check for when the Cuffer is a Chaos
                    case Team.CHI:
                    {
                        switch (team2)
                        {
                            case Team.MTF:
                            case Team.RSC:
                            case Team.CDP when ConfigFile.ServerConfig.GetBool("ci_can_cuff_class_d"):
                                flag = true;
                                break;
                        }

                        break;
                    }
                    //Check for when the Cuffer is a Mtf
                    case Team.MTF:
                    {
                        switch (team2)
                        {
                            case Team.CHI:
                            case Team.CDP:
                            case Team.RSC when ConfigFile.ServerConfig.GetBool("mtf_can_cuff_researchers"):
                                flag = true;
                                break;
                        }

                        break;
                    }
                }

                //Event
                var cuffer = __instance.MyReferenceHub;
                var target2 = handcuffs.MyReferenceHub;
                Events.InvokePlayerCuffedEvent(cuffer, target2, ref flag);

                if (!flag) return false;

                __instance.ClearTarget();
                handcuffs.NetworkCufferId = __instance.MyReferenceHub.queryProcessor.PlayerId;

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