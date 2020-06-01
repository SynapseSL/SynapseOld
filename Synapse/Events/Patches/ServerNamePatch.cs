using Harmony;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(ServerConsole), nameof(ServerConsole.ReloadServerName))]
    public static class ServerNamePatch
    {
        public static void Postfix(ServerConsole __instance)
        {
            ServerConsole._serverName = ServerConsole._serverName.Replace("<size=1>SM119.0.0</size>", "");
            ServerConsole._serverName += $" <color=#66ff3300><size=1>SMSynapse-ModLoader v.0.1-Alpha</size></color>";
            Log.Info(ServerConsole._serverName);
        }
    }
}
