using System;
using System.Collections.Generic;
using MEC;
using System.IO;
using System.Linq;
using System.Reflection;
using Synapse.Events.Patches;

namespace Synapse
{
    public static class PluginManager
    {
        // Variables
        private static readonly List<Assembly> LoadedDependencies = new List<Assembly>();
        private static readonly List<Plugin> Plugins = new List<Plugin>();
        private static string SynapseDirectory { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"Synapse");
        public static string MainPluginDirectory { get; } = Path.Combine(SynapseDirectory, "Plugins");
        public static string DependenciesDirectory { get; } = Path.Combine(SynapseDirectory,"dependencies");
        public static string ConfigDirectory { get; } = Path.Combine(SynapseDirectory, "ServerConfigs");
        public static string ServerPluginDirectoty { get; internal set; }

        // Methods
        public static IEnumerator<float> LoadPlugins()
        {
            yield return Timing.WaitForSeconds(0.5f);

            LoadDependencies();

            var serverPluginDirectory = Path.Combine(MainPluginDirectory, $"Server{ServerConsole.Port} Plugins");
            ServerPluginDirectoty = serverPluginDirectory;

            if (!Directory.Exists(serverPluginDirectory))
                Directory.CreateDirectory(serverPluginDirectory);
            
            var plugins = Directory.GetFiles(serverPluginDirectory);

            foreach (var plugin in plugins)
            {
                if (plugin.EndsWith("Synapse.dll")) continue;

                if (plugin.EndsWith(".dll")) LoadPlugin(plugin);
            }

            HarmonyPatch();
            try
            {
                PlayerManager.localPlayer.GetComponent<ServerConsole>().RefreshServerName();
            }
            catch (Exception e)
            {
                Log.Error($"NameRefreh Error : {e}");
            }

            var configPath = Path.Combine(ConfigDirectory, $"Server{ServerConsole.Port}-config.yml");

            if (!File.Exists(configPath))
                File.Create(configPath).Close();

            Plugin.Config = new YamlConfig(configPath);

            OnEnable();
        }

        private static void LoadDependencies()
        {
            Log.Info("Loading Dependencies...");
            var depends = Directory.GetFiles(DependenciesDirectory);

            foreach (string dll in depends)
            {
                if (!dll.EndsWith(".dll")) continue;

                if (IsLoaded(dll))
                    return;

                var assembly = Assembly.LoadFrom(dll);
                LoadedDependencies.Add(assembly);
                Log.Info($"Successfully loaded {dll}");
            }
        }

        private static bool IsLoaded(string path)
        {
            return LoadedDependencies.Any(assembly => assembly.Location == path);
        }

        private static void LoadPlugin(string pluginPath)
        {
            Log.Info($"Loading {pluginPath}");
            try
            {
                var file = ReadFile(pluginPath);
                var assembly = Assembly.Load(file);

                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsAbstract)
                    {
                        continue;
                    }

                    if (type.FullName == null) continue;

                    if (!typeof(Plugin).IsAssignableFrom(type)) continue;

                    var plugin = Activator.CreateInstance(type);

                    if (!(plugin is Plugin p)) continue;

                    Plugins.Add(p);
                    Log.Info($"Successfully loaded {p.GetName}");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Error while initializing {pluginPath}: {e}");
            }
        }

        private static void OnEnable()
        {
            foreach (var plugin in Plugins)
            {
                try
                {
                    plugin.OwnPluginFolder = Path.Combine(ServerPluginDirectoty, plugin.GetName);
                    plugin.OnEnable();
                }
                catch (Exception e)
                {
                    Log.Error($"Plugin {plugin.GetName} threw an exception while enabling {e}");
                }
            }
        }

        private static void HarmonyPatch()
        {
            try
            {
                var patchHandler = new PatchHandler();
                patchHandler.PatchMethods();
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
