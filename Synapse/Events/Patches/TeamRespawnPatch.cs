using System;
using System.Collections.Generic;
using System.Linq;
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
				int num = 0;
				IEnumerable<GameObject> enumerable = Enumerable.Where<GameObject>(PlayerManager.players, (GameObject item) => item.GetComponent<CharacterClassManager>().CurClass == RoleType.Spectator && !item.GetComponent<ServerRoles>().OverwatchEnabled);

				if (__instance.priorityMTFRespawn)
				{
					enumerable = Enumerable.OrderBy<GameObject, long>(enumerable, (GameObject item) => item.GetComponent<CharacterClassManager>().DeathTime);
				}

				int num2 = __instance.nextWaveIsCI ? __instance.maxCIRespawnAmount : __instance.maxMTFRespawnAmount;
				if (GameCore.ConfigFile.ServerConfig.GetBool("respawn_tickets_enable", true))
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
					else
					{
						num2 = Mathf.Min(num2, __instance.NextWaveRespawnTickets);
					}
				}
				List<GameObject> list = Enumerable.ToList<GameObject>(Enumerable.Take<GameObject>(enumerable, num2));
				__instance.NextWaveRespawnTickets -= num2 - list.Count;
				if (GameCore.ConfigFile.ServerConfig.GetBool("use_crypto_rng", false))
				{
					list.ShuffleListSecure<GameObject>();
				}
				else
				{
					list.ShuffleList<GameObject>();
				}
				__instance.playersToNTF.Clear();
				if (__instance.nextWaveIsCI && AlphaWarheadController.Host.detonated)
				{
					__instance.nextWaveIsCI = false;
				}
				foreach (GameObject gameObject in list)
				{
					if (!(gameObject == null))
					{
						num++;
						if (__instance.nextWaveIsCI)
						{
							__instance.GetComponent<CharacterClassManager>().SetPlayersClass(RoleType.ChaosInsurgency, gameObject, false, false);
							ServerLogs.AddLog(ServerLogs.Modules.ClassChange, gameObject.GetComponent<NicknameSync>().MyNick + " (" + gameObject.GetComponent<CharacterClassManager>().UserId + ") respawned as Chaos Insurgency agent.", ServerLogs.ServerLogType.GameEvent);
						}
						else
						{
							__instance.playersToNTF.Add(gameObject);
						}
					}
				}
				if (num > 0)
				{
					ServerLogs.AddLog(ServerLogs.Modules.ClassChange, (__instance.nextWaveIsCI ? "Chaos Insurgency" : "MTF") + " respawned!", ServerLogs.ServerLogType.GameEvent);
					if (__instance.nextWaveIsCI)
					{
						__instance.Invoke("CmdDelayCIAnnounc", 1f);
					}
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
