using System;
using GameCore;
using Harmony;
using Synapse.Api;
using UnityEngine;

// ReSharper disable SwitchStatementMissingSomeEnumCasesNoDefault

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.CallCmdRegisterEscape))]
    public static class PlayerEscapePatch
    {
        public static bool Prefix(CharacterClassManager __instance)
        {
            try
            {
                //RateLimit Check
                if (!__instance._interactRateLimit.CanExecute()) return false;

                //Position Check
                if (Vector3.Distance(__instance.transform.position, __instance.GetComponent<Escape>().worldPosition) >=
                    Escape.radius * 2) return false;

                //Event vars
                var player = __instance.GetPlayer();
                var spawnRole = player.Role;
                var cufferRole = RoleType.None;
                var allow = true;
                var isCuffed = false;

                //Cuff Check
                var flag = false;
                var component = __instance.GetComponent<Handcuffs>();
                if (component.CufferId >= 0)
                {
                    var component2 = component.GetCuffer(component.CufferId).GetComponent<CharacterClassManager>();

                    cufferRole = component2.NetworkCurClass;
                    isCuffed = true;

                    if (ConfigFile.ServerConfig.GetBool("cuffed_escapee_change_team", true))
                    {
                        if (__instance.CurClass == RoleType.Scientist &&
                            (component2.CurClass == RoleType.ChaosInsurgency || component2.CurClass == RoleType.ClassD))
                            flag = true;

                        if (__instance.CurClass == RoleType.ClassD &&
                            (component2.CurRole.team == Team.MTF || component2.CurClass == RoleType.Scientist))
                            flag = true;
                    }
                }

                //TeamCheck
                Respawning.RespawnTickets singleton = Respawning.RespawnTickets.Singleton;
                var team = __instance.CurRole.team;
                switch (team)
                {
                    case Team.CDP when flag:
                        spawnRole = RoleType.NtfCadet;
                        break;
                    case Team.CDP:
                    case Team.RSC when flag:
                        spawnRole = RoleType.ChaosInsurgency;
                        break;
                    case Team.RSC:
                        spawnRole = RoleType.NtfScientist;
                        break;
                }

                //PlayerEscapeEvent
                Events.InvokePlayerEscapeEvent(player, ref allow, ref spawnRole, cufferRole, isCuffed);

                if (!allow) return false;

                if (spawnRole == RoleType.None || spawnRole == __instance.NetworkCurClass) return false;
                __instance.SetPlayersClass(spawnRole, __instance.gameObject, false, true);
                switch (__instance.CurRole.team)
                {
                    case Team.MTF:
                        RoundSummary.escaped_scientists++;
                        singleton.GrantTickets(Respawning.SpawnableTeamType.NineTailedFox, GameCore.ConfigFile.ServerConfig.GetInt("respawn_tickets_mtf_classd_cuffed_count", 1), false);
                        break;
                    case Team.CHI:
                        RoundSummary.escaped_ds++;
                        singleton.GrantTickets(Respawning.SpawnableTeamType.ChaosInsurgency, GameCore.ConfigFile.ServerConfig.GetInt("respawn_tickets_ci_classd_count", 1), false);
                        break;
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