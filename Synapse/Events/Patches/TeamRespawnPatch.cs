using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;
using Respawning;
using Synapse.Api;
using UnityEngine;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(RespawnManager), nameof(RespawnManager.Spawn))]
    public static class TeamRespawnPatch
    {
        public static bool Prefix(Respawning.RespawnManager __instance)
        {
            try
            {
				if (!RespawnWaveGenerator.SpawnableTeams.TryGetValue(__instance.NextKnownTeam, out SpawnableTeam spawnableTeam) || __instance.NextKnownTeam == SpawnableTeamType.None)
				{
					ServerConsole.AddLog("Fatal error. Team '" + __instance.NextKnownTeam.ToString() + "' is undefined.", (ConsoleColor)12);
					return false;
				}
				List<ReferenceHub> list = Enumerable.ToList<ReferenceHub>(Enumerable.Where<ReferenceHub>(ReferenceHub.GetAllHubs().Values, (ReferenceHub item) => item.characterClassManager.CurClass == RoleType.Spectator && !item.serverRoles.OverwatchEnabled));
				if (__instance._prioritySpawn)
				{
					list = Enumerable.ToList<ReferenceHub>(Enumerable.OrderBy<ReferenceHub, long>(list, (ReferenceHub item) => item.characterClassManager.DeathTime));
				}
				else
				{
					list.ShuffleList<ReferenceHub>();
				}
				RespawnTickets singleton = RespawnTickets.Singleton;
				int num = singleton.GetAvailableTickets(__instance.NextKnownTeam);
				if (num == 0)
				{
					num = singleton.DefaultTeamAmount;
					RespawnTickets.Singleton.GrantTickets(singleton.DefaultTeam, singleton.DefaultTeamAmount, true);
				}
				int num2 = Mathf.Min(num, spawnableTeam.MaxWaveSize);
				while (list.Count > num2)
				{
					list.RemoveAt(list.Count - 1);
				}
				list.ShuffleList<ReferenceHub>();
				List<ReferenceHub> list2 = ListPool<ReferenceHub>.Rent();

				var Playerlist = new List<Player>();
				foreach (ReferenceHub hub in list)
					Playerlist.Add(hub.GetPlayer());

				Events.InvokeTeamRespawnEvent(ref Playerlist,ref __instance.NextKnownTeam);

				if (__instance.NextKnownTeam == SpawnableTeamType.None)
					return false;

				foreach (var player in Playerlist)
				{
					try
					{
						RoleType classid = spawnableTeam.ClassQueue[Mathf.Min(list2.Count, spawnableTeam.ClassQueue.Length - 1)];
						player.ClassManager.SetPlayersClass(classid, player.gameObject, false, false);
						list2.Add(player.Hub);
						ServerLogs.AddLog(ServerLogs.Modules.ClassChange, string.Concat(new string[]
						{
						"Player ",
						player.Hub.LoggedNameFromRefHub(),
						" respawned as ",
						classid.ToString(),
						"."
						}), ServerLogs.ServerLogType.GameEvent, false);
					}
					catch (Exception ex)
					{
						if (player != null)
						{
							ServerLogs.AddLog(ServerLogs.Modules.ClassChange, "Player " + player.Hub.LoggedNameFromRefHub() + " couldn't be spawned. Err msg: " + ex.Message, ServerLogs.ServerLogType.GameEvent, false);
						}
						else
						{
							ServerLogs.AddLog(ServerLogs.Modules.ClassChange, "Couldn't spawn a player - target's ReferenceHub is null.", ServerLogs.ServerLogType.GameEvent, false);
						}
					}
				}
				if (list2.Count > 0)
				{
					ServerLogs.AddLog(ServerLogs.Modules.ClassChange, string.Concat(new object[]
					{
					"RespawnManager has successfully spawned ",
					list2.Count,
					" players as ",
					__instance.NextKnownTeam.ToString(),
					"!"
					}), ServerLogs.ServerLogType.GameEvent, false);
					RespawnTickets.Singleton.GrantTickets(__instance.NextKnownTeam, -list2.Count * spawnableTeam.TicketRespawnCost, false);
					Respawning.NamingRules.UnitNamingRule unitNamingRule;
					if (Respawning.NamingRules.UnitNamingRules.TryGetNamingRule(__instance.NextKnownTeam, out unitNamingRule))
					{
						string text;
						unitNamingRule.GenerateNew(__instance.NextKnownTeam, out text);
						foreach (ReferenceHub referenceHub2 in list2)
						{
							referenceHub2.characterClassManager.NetworkCurSpawnableTeamType = (byte)__instance.NextKnownTeam;
							referenceHub2.characterClassManager.NetworkCurUnitName = text;
						}
						unitNamingRule.PlayEntranceAnnouncement(text);
					}
					RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.UponRespawn, __instance.NextKnownTeam);
				}
				__instance.NextKnownTeam = SpawnableTeamType.None;

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