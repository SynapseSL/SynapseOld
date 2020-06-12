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
        public static bool Prefix(QueryProcessor __instance, ref string query, bool encrypted)
        {
            try
            {
                Events.InvokeConsoleCommandEvent(__instance.GetPlayer(), query, out var color,
                    out var returning);

                if (string.IsNullOrEmpty(color))
                    color = "red";

                if (!string.IsNullOrEmpty(returning))
                    __instance.GCT.SendToClient(__instance.connectionToClient, returning, color);

                return false;
            }
            catch (Exception e)
            {
                Log.Error($"ConsoleCommand Event Error: {e}");
                return true;
            }
        }
    }
}