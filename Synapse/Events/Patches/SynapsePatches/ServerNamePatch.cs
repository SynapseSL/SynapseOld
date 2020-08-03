using Harmony;
using Synapse.Config;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(ServerConsole), nameof(ServerConsole.ReloadServerName))]
    public static class ServerNamePatch
    {
        public static void Postfix()
        {
            if (!SynapseConfigs.NameTracking) return;

            ServerConsole._serverName += $" <color=#00000000><size=1>Synapse-ModLoader {Synapse.Version}</size></color>";
        }
    }
}