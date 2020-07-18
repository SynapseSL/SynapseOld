using Harmony;
using Synapse.Configs;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(ServerConsole), nameof(ServerConsole.ReloadServerName))]
    public static class ServerNamePatch
    {
        public static void Postfix()
        {
            if (!SynapseConfigs.Nametracking) return;

            ServerConsole._serverName += $" <color=#00000000><size=1>SMSynapse-ModLoader {MainLoader.version}</size></color>";
        }
    }
}