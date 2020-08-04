using System;
using System.Linq;
using Harmony;
using NorthwoodLib.Pools;
using Respawning;
using Respawning.NamingRules;
using Synapse.Api;
using UnityEngine;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(RespawnManager), nameof(RespawnManager.Spawn))]
    public static class TeamRespawnPatch
    {
        public static bool Prefix(RespawnManager __instance)
        {
            try
            {
				if (!RespawnWaveGenerator.SpawnableTeams.TryGetValue(__instance.NextKnownTeam, out var spawnableTeam) || __instance.NextKnownTeam == SpawnableTeamType.None)
				{
					ServerConsole.AddLog("Fatal error. Team '" + __instance.NextKnownTeam.ToString() + "' is undefined.", (ConsoleColor)12);
					return false;
				}
				var list = ReferenceHub.GetAllHubs().Values.Where((item) => item.characterClassManager.CurClass == RoleType.Spectator && !item.serverRoles.OverwatchEnabled).ToList();
				if (__instance._prioritySpawn)
				{
					list = list.OrderBy((item) => item.characterClassManager.DeathTime).ToList();
				}
				else
				{
					list.ShuffleList();
				}
				var singleton = RespawnTickets.Singleton;
				var num = singleton.GetAvailableTickets(__instance.NextKnownTeam);
				if (num == 0)
				{
					num = singleton.DefaultTeamAmount;
					RespawnTickets.Singleton.GrantTickets(singleton.DefaultTeam, singleton.DefaultTeamAmount, true);
				}
				var num2 = Mathf.Min(num, spawnableTeam.MaxWaveSize);
				while (list.Count > num2)
				{
					list.RemoveAt(list.Count - 1);
				}
				list.ShuffleList();
				var list2 = ListPool<ReferenceHub>.Shared.Rent();

				var playerList = list.Select(hub => hub.GetPlayer()).ToList();

				Events.InvokeTeamRespawnEvent(ref playerList,ref __instance.NextKnownTeam);

				if (__instance.NextKnownTeam == SpawnableTeamType.None)
					return false;

				foreach (var player in playerList)
				{
					try
					{
						var classId = spawnableTeam.ClassQueue[Mathf.Min(list2.Count, spawnableTeam.ClassQueue.Length - 1)];
						player.ClassManager.SetPlayersClass(classId, player.gameObject);
						list2.Add(player.Hub);
						ServerLogs.AddLog(ServerLogs.Modules.ClassChange, string.Concat(new string[]
						{
						"Player ",
						player.Hub.LoggedNameFromRefHub(),
						" respawned as ",
						classId.ToString(),
						"."
						}), ServerLogs.ServerLogType.GameEvent);
					}
					catch (Exception ex)
					{
						if (player != null)
						{
							ServerLogs.AddLog(ServerLogs.Modules.ClassChange, "Player " + player.Hub.LoggedNameFromRefHub() + " couldn't be spawned. Err msg: " + ex.Message, ServerLogs.ServerLogType.GameEvent);
						}
						else
						{
							ServerLogs.AddLog(ServerLogs.Modules.ClassChange, "Couldn't spawn a player - target's ReferenceHub is null.", ServerLogs.ServerLogType.GameEvent);
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
					}), ServerLogs.ServerLogType.GameEvent);
					RespawnTickets.Singleton.GrantTickets(__instance.NextKnownTeam, -list2.Count * spawnableTeam.TicketRespawnCost);
					if (UnitNamingRules.TryGetNamingRule(__instance.NextKnownTeam, out var unitNamingRule))
					{
						unitNamingRule.GenerateNew(__instance.NextKnownTeam, out var text);
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