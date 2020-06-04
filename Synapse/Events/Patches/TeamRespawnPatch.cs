using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using Harmony;
using UnityEngine;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(MTFRespawn), nameof(MTFRespawn.RespawnDeadPlayers))]
    public static class TeamRespawnPatch
    {
        public static bool Prefix(MTFRespawn __instance)
        {
            try
            {
                var num = 0;
                var enumerable = PlayerManager.players.Where(item =>
                    item.GetComponent<CharacterClassManager>().CurClass == RoleType.Spectator &&
                    !item.GetComponent<ServerRoles>().OverwatchEnabled);

                if (__instance.priorityMTFRespawn)
                    enumerable = enumerable.OrderBy(item => item.GetComponent<CharacterClassManager>().DeathTime);

                var num2 = __instance.nextWaveIsCI ? __instance.maxCIRespawnAmount : __instance.maxMTFRespawnAmount;
                if (ConfigFile.ServerConfig.GetBool("respawn_tickets_enable", true))
                {
                    if (__instance.NextWaveRespawnTickets == 0)
                    {
                        if (__instance.nextWaveIsCI)
                        {
                            __instance._ciDisabled = true;
                            return false;
                        }

                        RoundSummary.singleton.ForceEnd();
                        return false;
                    }

                    num2 = Mathf.Min(num2, __instance.NextWaveRespawnTickets);
                }

                var list = enumerable.Take(num2).ToList();

                //Event
                var allow = true;
                var respawnlist = new List<ReferenceHub>();
                var usetickets = true;

                foreach (GameObject player in list)
                    respawnlist.Add(player.GetComponent<ReferenceHub>());

                Events.InvokeTeamRespawnEvent(ref respawnlist,ref __instance.nextWaveIsCI,ref allow,ref usetickets);

                if (!allow) return false;
                list.Clear();
                foreach (ReferenceHub hub in respawnlist)
                    list.Add(hub.gameObject);

                if (usetickets) __instance.NextWaveRespawnTickets -= num2 - list.Count;

                if (ConfigFile.ServerConfig.GetBool("use_crypto_rng"))
                    list.ShuffleListSecure();
                else
                    list.ShuffleList();

                __instance.playersToNTF.Clear();
                //if (__instance.nextWaveIsCI && AlphaWarheadController.Host.detonated) __instance.nextWaveIsCI = false;
                foreach (var gameObject in list.Where(gameObject => !(gameObject == null)))
                {
                    num++;
                    if (__instance.nextWaveIsCI)
                    {
                        __instance.GetComponent<CharacterClassManager>()
                            .SetPlayersClass(RoleType.ChaosInsurgency, gameObject);
                        ServerLogs.AddLog(ServerLogs.Modules.ClassChange,
                            gameObject.GetComponent<NicknameSync>().MyNick + " (" +
                            gameObject.GetComponent<CharacterClassManager>().UserId +
                            ") respawned as Chaos Insurgency agent.", ServerLogs.ServerLogType.GameEvent);
                    }
                    else
                    {
                        __instance.playersToNTF.Add(gameObject);
                    }
                }

                if (num > 0)
                {
                    ServerLogs.AddLog(ServerLogs.Modules.ClassChange,
                        (__instance.nextWaveIsCI ? "Chaos Insurgency" : "MTF") + " respawned!",
                        ServerLogs.ServerLogType.GameEvent);
                    if (__instance.nextWaveIsCI) __instance.Invoke("CmdDelayCIAnnounc", 1f);
                }

                __instance.SummonNTF();

                return false;
            }
            catch (Exception e)
            {
                Log.Error($"TeamRespawnEvent Error: {e}");
                return true;
            }
        }
    }
}