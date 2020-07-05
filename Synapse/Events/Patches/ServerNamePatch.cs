using Harmony;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(ServerConsole), nameof(ServerConsole.ReloadServerName))]
    public static class ServerNamePatch
    {
        public static void Postfix()
        {
            ServerConsole._serverName = ServerConsole._serverName.Replace("<size=1>SM119.0.0</size>", "");
            ServerConsole._serverName += " <color=green><size=1>SMSynapse-ModLoader v.1.0-beta</size></color>";
        }
    }
}