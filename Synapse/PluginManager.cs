using System;
using System.Collections.Generic;
using MEC;
using System.IO;
using System.Reflection;

namespace Synapse
{
    public class PluginManager
    {
        public static List<Assembly> LoadedDependencies { get; } = new List<Assembly>();
        public static string SynapseDirectoty { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"Synapse");
        public static string MainPluginDirectoty { get; } = Path.Combine(SynapseDirectoty, "Plugins");
        public static string ServerPluginDirectory { get; } = Path.Combine(MainPluginDirectoty,$"Server{ServerStatic.ServerPort} Plugins");
        public static string DependenciesDirectory { get; } = Path.Combine(SynapseDirectoty,"dependencies");
        public static string ConfigDirectory { get; } = Path.Combine(SynapseDirectoty, "ServerConfigs");
        public static string ConfigPath { get; } = ConfigDirectory + $"Server{ServerStatic.ServerPort}-config.yml";

        public static IEnumerator<float> LoadPlugins()
        {
            yield return Timing.WaitForSeconds(0.5f);


        }

        private static void LoadDependencies()
        {
            string[] depends = Directory.GetFiles(DependenciesDirectory);

            foreach (string dll in depends)
            {
                if (!dll.EndsWith(".dll")) continue;

                if (IsLoaded(dll))
                    return;
            }
        }

        private static bool IsLoaded(string path)
        {
            foreach (Assembly assembly in LoadedDependencies)
            {
                if (assembly.Location == path)
                    return true;
            }
            return false;
        }
    }
}
