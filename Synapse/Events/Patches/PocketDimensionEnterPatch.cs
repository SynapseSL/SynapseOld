using System;
using System.Runtime.CompilerServices;
using CustomPlayerEffects;
using Harmony;
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

                var player = ply.GetComponent<ReferenceHub>();
                var charClassManager = player.characterClassManager;

                if (charClassManager == null || charClassManager.GodMode || charClassManager.IsAnyScp()) return false;

                if (!ServerTime.CheckSynchronization(t) || !__instance.iAm106 ||
                    Vector3.Distance(__instance.hub.playerMovementSync.RealModelPosition, ply.transform.position) >=
                    3f || !charClassManager.IsHuman()) return false;

                if (Scp106PlayerScript.blastDoor.isClosed)
                {
                    __instance.hub.characterClassManager.RpcPlaceBlood(ply.transform.position, 1, 2f);
                    __instance.hub.playerStats.HurtPlayer(
                        new PlayerStats.HitInfo(500f,
                            $"{__instance.GetComponent<NicknameSync>().MyNick} ({__instance.hub.characterClassManager.UserId})",
                            DamageTypes.Scp106, __instance.GetComponent<RemoteAdmin.QueryProcessor>().PlayerId), ply);
                }
                else
                {
                    //TODO: Implement SCP079 Shit
                }

                var canEnter = true;
                Events.InvokePocketDimensionEnterEvent(player, ref canEnter);
                if (!canEnter) return false;
                
                //TODO: Implement Damage Event
                
                __instance.hub.playerStats.HurtPlayer(
                    new PlayerStats.HitInfo(40f,
                        $"{__instance.GetComponent<NicknameSync>().MyNick} ({__instance.hub.characterClassManager.UserId})",
                        DamageTypes.Scp106, __instance.GetComponent<RemoteAdmin.QueryProcessor>().PlayerId), ply);
                
                player.playerMovementSync.OverridePosition(Vector3.down * 1998.5f, 0f, true);
                player.playerEffectsController.GetEffect<Corroding>().IsInPd = true;
                player.playerEffectsController.EnableEffect<Corroding>();

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