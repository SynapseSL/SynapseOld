using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using GameCore;
using Harmony;
using UnityEngine;
using Console = System.Console;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(RoundSummary), nameof(RoundSummary.Start))]
    public class CheckRoundEndPatch
    {
        private static readonly MethodInfo
            CustomProcess = SymbolExtensions.GetMethodInfo(() => ProcessServerSide(null));
        
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr)
        {
            var codes = new List<CodeInstruction>(instr);

            foreach (var code in codes.Select((x,i) => new {Value =x, Index = i }))
            {
                if (code.Value.opcode != OpCodes.Call) continue;

                if (code.Value.operand != null && code.Value.operand is MethodBase methodBase &&
                    methodBase.Name == nameof(RoundSummary._ProcessServerSideCode))
                {
                    codes[code.Index].operand = CustomProcess;
                }
            }

            return codes.AsEnumerable();
        }

        public static IEnumerator<float> ProcessServerSide(RoundSummary instance)
        {
            Log.Info("Cool");
            var roundSummary = instance;

            Log.Info(roundSummary.ToString());

            while (roundSummary != null)
            {
                while (RoundSummary.RoundLock || !RoundSummary.RoundInProgress() ||
                       roundSummary.keepRoundOnOne && PlayerManager.players.Count < 2) yield return 0.0f;
                
                var newList = new RoundSummary.SumInfo_ClassList();

                foreach (var chrClassManager in PlayerManager.players.Where(gameObject => gameObject != null).Select(gameObject => gameObject.GetComponent<CharacterClassManager>()).Where(chrClassManager => chrClassManager.Classes.CheckBounds(chrClassManager.CurClass)))
                {
                    // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                    switch (chrClassManager.Classes.SafeGet(chrClassManager.CurClass).team)
                    {
                        case Team.SCP:
                            if (chrClassManager.CurClass == RoleType.Scp0492)
                            {
                                newList.zombies++;
                                continue;
                            }

                            newList.scps_except_zombies++;
                            continue;
                        case Team.MTF:
                            newList.mtf_and_guards++;
                            continue;
                        case Team.CHI:
                            newList.chaos_insurgents++;
                            continue;
                        case Team.RSC:
                            newList.scientists++;
                            continue;
                        case Team.CDP:
                            newList.class_ds++;
                            continue;
                        default:
                            continue;
                    }
                }

                newList.warhead_kills =
                    AlphaWarheadController.Host.detonated ? AlphaWarheadController.Host.warheadKills : -1;

                yield return float.NegativeInfinity;
                newList.time = (int) Time.realtimeSinceStartup;
                yield return float.NegativeInfinity;

                RoundSummary.roundTime = newList.time - roundSummary.classlistStart.time;

                var mtfSum = newList.mtf_and_guards + newList.scientists;
                var chaosSum = newList.chaos_insurgents + newList.class_ds;
                var scpSum = newList.scps_except_zombies + newList.zombies;

                Log.Warn($"mtfSum: {mtfSum} | chaosSum: {chaosSum} | scpSum: {scpSum}");

                var escapedDs = (float)(roundSummary.classlistStart.class_ds == 0 ? 0 : (RoundSummary.escaped_ds + newList.class_ds) / roundSummary.classlistStart.class_ds);
                var escapedScientists = (float)(roundSummary.classlistStart.scientists == 0 ? 1 : (RoundSummary.escaped_scientists + newList.scientists) / roundSummary.classlistStart.scientists);

                var allow = true;
                var forceEnd = false;
                var teamChanged = false;
                var team = RoundSummary.LeadingTeam.Draw;

                try
                {
                    Events.InvokeCheckRoundEnd(ref forceEnd, ref allow, ref team, ref teamChanged);
                }
                catch (Exception e)
                {
                    Log.Error($"CheckRoundEnd err: {e}");
                    continue;
                }

                if (forceEnd) roundSummary.roundEnded = true;
                
                if(!allow) continue;

                if (newList.class_ds == 0 && mtfSum == 0)
                {
                    roundSummary.roundEnded = true;
                }
                
                else if (mtfSum == 0 && PlayerManager.localPlayer.GetComponent<MTFRespawn>().MtfRespawnTickets == 0)
                {
                    roundSummary.roundEnded = true;
                }

                else
                {
                    //Okay. SCP hat hier einfach wirklich nur Staub gefressen oder so.
                    var checkVar = 0;

                    if (mtfSum > 0) checkVar++;
                    if (chaosSum > 0) checkVar++;
                    if (scpSum > 0) checkVar++;

                    if (checkVar <= 1) roundSummary.roundEnded = true;
                }
                
                
                if (!roundSummary.roundEnded) continue;
                var leadingTeam = RoundSummary.LeadingTeam.Draw;

                if (mtfSum > 0)
                {
                    if (RoundSummary.escaped_ds == 0 && RoundSummary.escaped_scientists != 0)
                        leadingTeam = RoundSummary.LeadingTeam.FacilityForces;
                }
                else
                    leadingTeam = RoundSummary.escaped_ds != 0
                        ? RoundSummary.LeadingTeam.ChaosInsurgency
                        : RoundSummary.LeadingTeam.Anomalies;

                if (teamChanged) leadingTeam = team;

                var text = $"Round finished! Anomalies:{scpSum} | Chaos: {chaosSum} | Facility Forces: {mtfSum} | D escaped percentage: {escapedDs} | S escaped percentage: {escapedScientists}";
                
                GameCore.Console.AddLog(text, Color.gray);
                ServerLogs.AddLog(ServerLogs.Modules.Logger, text, ServerLogs.ServerLogType.GameEvent);
                
                for (byte i = 0; i < 75; i += 1)
                {
                    yield return 0f;
                }
                var timeToRoundRestart = Mathf.Clamp(ConfigFile.ServerConfig.GetInt("auto_round_restart_time", 10), 5, 1000);
                if (roundSummary != null)
                {
                    roundSummary.RpcShowRoundSummary(roundSummary.classlistStart, newList, leadingTeam, RoundSummary.escaped_ds, RoundSummary.escaped_scientists, RoundSummary.kills_by_scp, timeToRoundRestart);
                }
                int num7;
                for (var j = 0; j < 50 * (timeToRoundRestart - 1); j = num7 + 1)
                {
                    yield return 0f;
                    num7 = j;
                }
                roundSummary.RpcDimScreen();
                for (byte i = 0; i < 50; i += 1)
                {
                    yield return 0f;
                }
                PlayerManager.localPlayer.GetComponent<PlayerStats>().Roundrestart();
            }
        }
    }
}