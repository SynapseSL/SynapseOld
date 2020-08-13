using System;
using Harmony;
using RemoteAdmin;
using Synapse.Api;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(QueryProcessor), nameof(QueryProcessor.ProcessGameConsoleQuery))]
    public class ConsoleCommandPatch
    {
        // ReSharper disable once InconsistentNaming
        public static bool Prefix(QueryProcessor __instance, string query)
        {
            try
            {
                Events.InvokeConsoleCommandEvent(__instance.GetPlayer(), query, out var allow);

                return allow;
            }
            catch (Exception e)
            {
                Log.Error($"ConsoleCommand Event Error: {e}");
                return true;
            }
        }
    }
}