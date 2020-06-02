using System;
using Harmony;
using Synapse.Api;
using UnityEngine;

namespace Synapse.Events.Classes
{
    [HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.CallCmdRegisterEscape))]
    public static class PlayerEscapePatch
    {
        public static bool Prefix(CharacterClassManager __instance)
        {
            try
            {
				//RateLimit Check
				if (!__instance._interactRateLimit.CanExecute(true)) return false;

				//Position Check
				if (Vector3.Distance(__instance.transform.position, __instance.GetComponent<Escape>().worldPosition) >= (float)(Escape.radius * 2)) return false;

				//Event vars
				var player = __instance.GetComponent<ReferenceHub>();
				var spawnRole = player.GetRole();
				var cufferRole = RoleType.None;
				var allow = true;
				var isCuffed = false;

				//Cuff Check
				bool flag = false;
				Handcuffs component = __instance.GetComponent<Handcuffs>();
				if (component.CufferId >= 0)
				{
					CharacterClassManager component2 = component.GetCuffer(component.CufferId).GetComponent<CharacterClassManager>();

					cufferRole = component2.NetworkCurClass;
					isCuffed = true;

					if (GameCore.ConfigFile.ServerConfig.GetBool("cuffed_escapee_change_team", true))
                    {
						if (__instance.CurClass == RoleType.Scientist && (component2.CurClass == RoleType.ChaosInsurgency || component2.CurClass == RoleType.ClassD))
							flag = true;

						if (__instance.CurClass == RoleType.ClassD && (component2.CurRole.team == Team.MTF || component2.CurClass == RoleType.Scientist))
							flag = true;
					}
				}

				//TeamCheck
				MTFRespawn component3 = PlayerManager.localPlayer.GetComponent<MTFRespawn>();
				Team team = __instance.CurRole.team;
				if (team == Team.CDP)
				{
					if (flag) spawnRole = RoleType.NtfCadet;
					else spawnRole = RoleType.ChaosInsurgency;
				}
				else if (team == Team.RSC)
				{
					if (flag) spawnRole = RoleType.ChaosInsurgency;
					else spawnRole = RoleType.NtfScientist;
				}

				//PlayerEscapeEvent
				Events.InvokePlayerEscapeEvent(player, ref allow, ref spawnRole, cufferRole, isCuffed);

				if (!allow) return false;

				if (spawnRole != RoleType.None && spawnRole != __instance.NetworkCurClass)
                {
					__instance.SetPlayersClass(spawnRole, __instance.gameObject, false, true);
					if (__instance.CurRole.team == Team.MTF)
                    {
						RoundSummary.escaped_scientists++;
						component3.MtfRespawnTickets += GameCore.ConfigFile.ServerConfig.GetInt("respawn_tickets_mtf_scientist_count", 1);
					}
					else if (__instance.CurRole.team == Team.CHI)
                    {
						RoundSummary.escaped_ds++;
						component3.ChaosRespawnTickets += GameCore.ConfigFile.ServerConfig.GetInt("respawn_tickets_ci_scientist_cuffed_count", 2);
					}
				}
				return false;
            }
            catch (Exception e)
            {
                Log.Error($"PlayerEscapeEvent Error: {e}");
                return true;
            }
        }
    }
}
