using System;
using Synapse.Api.Plugin;

namespace Synapse.Config
{
    public static class ConfigManager
    {
        internal static void InitializeConfigs()
        {
            Plugin.Config = new YamlConfig(Files.ServerConfigFile);
            SynapseConfigs.ReloadConfig();
        }

        internal static void ReloadAllConfigs()
        {
            SynapseConfigs.ReloadConfig();
            Plugin.Config = new YamlConfig(Files.ServerConfigFile);

            foreach (var plugin in Synapse.plugins)
                try
                {
                    plugin.InvokeConfigReloadEvent();
                }
                catch (Exception e)
                {
                    Log.Error($"Plugin {plugin.Details.Name} threw an exception while reloading {e}");
                }
        }
    }
}
