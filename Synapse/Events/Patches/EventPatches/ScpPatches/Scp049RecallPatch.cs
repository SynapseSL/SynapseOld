using System;
using System.Linq;
using Harmony;
using MEC;
using Mirror;
using PlayableScps;
using Synapse.Api;
using UnityEngine;
using Console = GameCore.Console;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(Scp049), nameof(Scp049.BodyCmd_ByteAndGameObject))]
    public class Scp049RecallPatch
    {
        // ReSharper disable once InconsistentNaming
        public static bool Prefix(Scp049 __instance, ref byte num, ref GameObject go)
        {
            try
            {
                if (num == 0)
                {
                    if (!__instance._interactRateLimit.CanExecute()) return false;
                    if (go == null) return false;
                    if (Vector3.Distance(go.transform.position, __instance.Hub.playerMovementSync.RealModelPosition) >=
                        Scp049.AttackDistance * 1.25f) return false;
                    __instance.Hub.playerStats.HurtPlayer(
                        new PlayerStats.HitInfo(4949f,
                            __instance.Hub.nicknameSync.MyNick + " (" + __instance.Hub.characterClassManager.UserId +
                            ")", DamageTypes.Scp049, __instance.Hub.queryProcessor.PlayerId), go);
                    Console.AddDebugLog("SCPCTRL", "SCP-049 | Sent 'death time' RPC", MessageImportance.LessImportant);
                    __instance.Hub.scpsController.RpcTransmit_Byte(0);
                    return false;
                }

                if (num != 1)
                {
                    if (num != 2) return false;
                    if (!__instance._interactRateLimit.CanExecute()) return false;
                    if (go == null) return false;
                    var component = go.GetComponent<Ragdoll>();
                    if (component == null) return false;
                    var target = PlayerManager.players.Select(ReferenceHub.GetHub)
                        .FirstOrDefault(hub => hub.queryProcessor.PlayerId == component.owner.PlayerId).GetPlayer();
                    if (target == null)
                    {
                        Console.AddDebugLog("SCPCTRL", "SCP-049 | Request 'finish recalling' rejected; no target found",
                            MessageImportance.LessImportant);
                        return false;
                    }

                    if (!__instance._recallInProgressServer ||
                        target.gameObject != __instance._recallObjectServer ||
                        __instance._recallProgressServer < 0.85f)
                    {
                        Console.AddDebugLog("SCPCTRL", "SCP-049 | Request 'finish recalling' rejected; Debug code: ",
                            MessageImportance.LessImportant);
                        Console.AddDebugLog("SCPCTRL",
                            "SCP-049 | CONDITION#1 " + (__instance._recallInProgressServer
                                ? "<color=green>PASSED</color>"
                                : "<color=red>ERROR</color> - " + __instance._recallInProgressServer),
                            MessageImportance.LessImportant, true);
                        Console.AddDebugLog("SCPCTRL",
                            "SCP-049 | CONDITION#2 " + (target == __instance._recallObjectServer
                                ? "<color=green>PASSED</color>"
                                : string.Concat("<color=red>ERROR</color> - ", target.PlayerId,
                                    "-",
                                    __instance._recallObjectServer == null
                                        ? "null"
                                        : ReferenceHub.GetHub(__instance._recallObjectServer).queryProcessor.PlayerId
                                            .ToString())), MessageImportance.LessImportant);
                        Console.AddDebugLog("SCPCTRL",
                            "SCP-049 | CONDITION#3 " + (__instance._recallProgressServer >= 0.85f
                                ? "<color=green>PASSED</color>"
                                : "<color=red>ERROR</color> - " + __instance._recallProgressServer),
                            MessageImportance.LessImportant, true);
                        return false;
                    }

                    if (target.Hub.characterClassManager.CurClass != RoleType.Spectator) return false;

                    //Event
                    var allow = true;
                    var role = RoleType.Scp0492;
                    float live = target.Hub.characterClassManager.Classes.Get(RoleType.Scp0492).maxHP;
                    Events.InvokeScp049RecallEvent(__instance.Hub.GetPlayer(), ref component, ref target, ref allow, ref role,
                        ref live);

                    Console.AddDebugLog("SCPCTRL", "SCP-049 | Request 'finish recalling' accepted",
                        MessageImportance.LessImportant);
                    RoundSummary.changed_into_zombies++;

                    var pos3 = component.transform.position;
                    pos3.y += 2;
                    target.Role = role;
                    Timing.CallDelayed(0.5f, () => target.Position = pos3);
                    target.GetComponent<PlayerStats>().Health = live;
                    if (component.CompareTag("Ragdoll")) NetworkServer.Destroy(component.gameObject);
                    __instance._recallInProgressServer = false;
                    __instance._recallObjectServer = null;
                    __instance._recallProgressServer = 0f;
                    return false;
                }

                if (!__instance._interactRateLimit.CanExecute()) return false;
                if (go == null) return false;
                var component2 = go.GetComponent<Ragdoll>();
                if (component2 == null)
                {
                    Console.AddDebugLog("SCPCTRL",
                        "SCP-049 | Request 'start recalling' rejected; provided object is not a dead body",
                        MessageImportance.LessImportant);
                    return false;
                }

                if (!component2.allowRecall)
                {
                    Console.AddDebugLog("SCPCTRL",
                        "SCP-049 | Request 'start recalling' rejected; provided object can't be recalled",
                        MessageImportance.LessImportant);
                    return false;
                }

                var referenceHub2 = PlayerManager.players.Select(ReferenceHub.GetHub).FirstOrDefault(hub2 =>
                    hub2 != null && hub2.queryProcessor.PlayerId == component2.owner.PlayerId);
                if (referenceHub2 == null)
                {
                    Console.AddDebugLog("SCPCTRL", "SCP-049 | Request 'start recalling' rejected; target not found",
                        MessageImportance.LessImportant);
                    return false;
                }

                if (Vector3.Distance(component2.transform.position,
                        __instance.Hub.PlayerCameraReference.transform.position) >=
                    Scp049.ReviveDistance * 1.3f) return false;
                Console.AddDebugLog("SCPCTRL", "SCP-049 | Request 'start recalling' accepted",
                    MessageImportance.LessImportant);
                __instance._recallObjectServer = referenceHub2.gameObject;
                __instance._recallProgressServer = 0f;
                __instance._recallInProgressServer = true;

                return false;
            }
            catch (Exception e)
            {
                Log.Error($"Scp049RecallEvent Error: {e}");
                return true;
            }
        }
    }
}