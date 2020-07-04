using System.IO;

namespace Synapse
{
    internal static class Configs
    {
        // Variables
        private static YamlConfig _config;

        // Configs
        internal static int RequiredForFemur;
        internal static bool RemoteKeyCard;
        internal static string JoinBroadcast;
        internal static string JoinTextHint;
        internal static ushort JoinMessageDuration;

        // Methods
        internal static void ReloadConfig()
        {
            if (_config == null) _config = new YamlConfig(Path.Combine(PluginManager.ServerConfigDirectory, "server-config.yml"));

            RequiredForFemur = _config.GetInt("synapse_femur",1);
            RemoteKeyCard = _config.GetBool("synapse_remote_keycard", false);
            JoinBroadcast = _config.GetString("synapse_join_broadcast", "");
            JoinTextHint = _config.GetString("synapse_join_texthint", "");
            JoinMessageDuration = _config.GetUShort("synapse_join_duration",5);
        }
    }
}
