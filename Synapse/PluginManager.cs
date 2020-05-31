using System;
using System.Collections.Generic;
using MEC;
using System.IO;
using System.Reflection;
using Synapse.Events.Patches;

namespace Synapse
{
    public class PluginManager
    {
        //Variablen
        private static List<Assembly> LoadedDependencies = new List<Assembly>();
        public static readonly List<Plugin> _plugins = new List<Plugin>();
        public static string SynapseDirectoty { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"Synapse");
        public static string MainPluginDirectoty { get; } = Path.Combine(SynapseDirectoty, "Plugins");
        public static string ServerPluginDirectory { get; } = Path.Combine(MainPluginDirectoty,$"Server{ServerConsole.Port} Plugins");
        public static string DependenciesDirectory { get; } = Path.Combine(SynapseDirectoty,"dependencies");
        public static string ConfigDirectory { get; } = Path.Combine(SynapseDirectoty, "ServerConfigs");
        public static string ConfigPath { get; } = Path.Combine(ConfigDirectory,$"Server{ServerConsole.Port}-config.yml");

        //Methoden
        public static IEnumerator<float> LoadPlugins()
        {
            yield return Timing.WaitForSeconds(0.5f);

            LoadDependencies();

            string[] plugins = Directory.GetFiles(ServerPluginDirectory);

            foreach (string plugin in plugins)
            {
                if (plugin.EndsWith("Synapse.dll")) continue;

                if (plugin.EndsWith(".dll")) LoadPlugin(plugin);
            }

            HarmonyPatch();
            OnEnable();
        }

        private static void LoadDependencies()
        {
            Log.Info("Dependencies werden geladen");
            string[] depends = Directory.GetFiles(DependenciesDirectory);

            foreach (string dll in depends)
            {
                if (!dll.EndsWith(".dll")) continue;

                if (IsLoaded(dll))
                    return;

                Assembly assembly = Assembly.LoadFrom(dll);
                LoadedDependencies.Add(assembly);
                Log.Info($"Succesfully loaded {dll}");
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

        private static void LoadPlugin(string pluginpath)
        {
            Log.Info($"Loading {pluginpath}");
            try
            {
                byte[] file = ReadFile(pluginpath);
                Assembly assembly = Assembly.Load(file);

                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsAbstract)
                    {
                        continue;
                    }

                    if (type.FullName == null) continue;

                    if (!typeof(Plugin).IsAssignableFrom(type)) continue;

                    object plugin = Activator.CreateInstance(type);

                    if (!(plugin is Plugin p)) continue;

                    _plugins.Add(p);
                    Log.Info($"Succesfully loaded {p.getName}");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Error while initializing {pluginpath}: {e}");
            }
        }

        private static void OnEnable()
        {
            foreach (Plugin plugin in _plugins)
            {
                try
                {
                    plugin.OnEnable();
                }
                catch (Exception e)
                {
                    Log.Error($"Plugin {plugin.getName} threw an exception while enabling {e}");
                }
            }
        }

        private static void HarmonyPatch()
        {
            try
            {
                var PatchHandler = new PatchHandler();
                PatchHandler.PatchMethods();
            }
            catch (Exception e)
            {
                Log.Error($"PatchError: {e}");
            }
        }

        public static byte[] ReadFile(string path)
        {
            FileStream fileStream = File.Open(path, FileMode.Open);
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);
                result = memoryStream.ToArray();
            }
            fileStream.Close();
            return result;
        }
    }
}
