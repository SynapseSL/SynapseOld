using System;
using System.Linq;
using CustomPlayerEffects;
using Harmony;
using Synapse.Api;
using UnityEngine;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(Scp106PlayerScript), nameof(Scp106PlayerScript.CallCmdMovePlayer))]
    public class PocketDimensionEnterPatch
    {
        public static bool Prefix(Scp106PlayerScript __instance, GameObject ply, int t)
        {
            try
            {
                if (!__instance._iawRateLimit.CanExecute()) return false;
                if (ply == null) return false;

                var player = ply.GetPlayer();
                var charClassManager = player.ClassManager;

                if (charClassManager == null || charClassManager.GodMode || charClassManager.IsAnyScp()) return false;

                if (!ServerTime.CheckSynchronization(t) || !__instance.iAm106 ||
                    Vector3.Distance(__instance.hub.playerMovementSync.RealModelPosition, ply.transform.position) >=
                    3f || !charClassManager.IsHuman()) return false;

                if (Scp106PlayerScript._blastDoor.isClosed)
                {
                    __instance.hub.characterClassManager.RpcPlaceBlood(ply.transform.position, 1, 2f);
                    __instance.hub.playerStats.HurtPlayer(
                        new PlayerStats.HitInfo(500f,
                            $"{__instance.GetComponent<NicknameSync>().MyNick} ({__instance.hub.characterClassManager.UserId})",
                            DamageTypes.Scp106, __instance.GetComponent<RemoteAdmin.QueryProcessor>().PlayerId), ply);
                }
                else
                {
                    foreach (var scp079PlayerScript in Scp079PlayerScript.instances)
                    {
                        var otherRoom = ply.GetComponent<Scp079PlayerScript>().GetOtherRoom();
                        var filter = new[]
                        {
                            Scp079Interactable.InteractableType.Door,
                            Scp079Interactable.InteractableType.Light,
                            Scp079Interactable.InteractableType.Lockdown,
                            Scp079Interactable.InteractableType.Tesla,
                            Scp079Interactable.InteractableType.ElevatorUse
                        };
                        var flag = false;
                        foreach (var zoneAndRoom in from scp079Interaction in scp079PlayerScript.ReturnRecentHistory(12f,
                            filter) from zoneAndRoom in scp079Interaction.interactable
                            .currentZonesAndRooms where zoneAndRoom.currentZone == otherRoom.currentZone &&
                                                        zoneAndRoom.currentRoom == otherRoom.currentRoom select zoneAndRoom)
                        {
                            flag = true;
                        }

                        if (flag)
                        {
                            scp079PlayerScript.RpcGainExp(ExpGainType.PocketAssist, player.ClassManager.CurClass);
                        }
                    }
                }

                var canEnter = true;
                Events.InvokePocketDimensionEnterEvent(player, ref canEnter);
                if (!canEnter) return false;

                __instance.hub.playerStats.HurtPlayer(
                    new PlayerStats.HitInfo(40f,
                        $"{__instance.GetComponent<NicknameSync>().MyNick} ({__instance.hub.characterClassManager.UserId})",
                        DamageTypes.Scp106, __instance.GetComponent<RemoteAdmin.QueryProcessor>().PlayerId), ply);

                player.Position = Vector3.down * 1998.5f;
                player.EffectsController.GetEffect<Corroding>().IsInPd = true;
                player.EffectsController.EnableEffect<Corroding>();

                return false;
            }
            catch (Exception e)
            {
                Log.Error($"PocketDimEnter Err: {e}");
                return true;
            }
        }
    }
}