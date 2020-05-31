using System;
using Harmony;
using RemoteAdmin;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(CommandProcessor), nameof(CommandProcessor.ProcessQuery), typeof(string), typeof(CommandSender))]
    public class RemoteAdminPatch
    {
        public static bool Prefix(ref string q, ref CommandSender sender)
        {
            try
            {
                if (q.Contains("REQUEST_DATA PLAYER_LIST SILENT"))
                    return true;

                bool allow = true;
                Events.InvokeRemoteCommandEvent(sender, q, ref allow);

                return allow;
            }
            catch (Exception e)
            {
                Log.Error($"RemoteAdminCommand Event Error: {e}");
                return true;
            }
        }
    }
}
