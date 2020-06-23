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

        // Methods
        internal static void ReloadConfig()
        {
            if (_config == null) _config = new YamlConfig(Path.Combine(PluginManager.ServerConfigDirectory, "server-config.yml"));

            RequiredForFemur = _config.GetInt("synapse_femur",1);
            RemoteKeyCard = _config.GetBool("synapse_remote_keycard", true);
        }
    }
}
