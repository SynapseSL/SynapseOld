using System.IO;

namespace Synapse
{
    internal static class Configs
    {
        //Variablen
        private static YamlConfig Config;

        //Configs
        internal static int requiredforFemur;

        //Methods
        internal static void ReloadConfig()
        {
            if (Config == null) Config = new YamlConfig(Path.Combine(PluginManager.ServerConfigDirectory, "server-config.yml"));

            requiredforFemur = Config.GetInt("synapse.femur",1);
        }
    }
}
