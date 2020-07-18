using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using MEC;
using Synapse.Events.Patches;
using Synapse.Configs;
using SynapseModLoader;
using Synapse.Api.Plugin;

namespace Synapse
{
    public static class SynapseManager
    {
        // Variables
        private static readonly List<Assembly> LoadedDependencies = new List<Assembly>();
        private static readonly List<Plugin> Plugins = new List<Plugin>();

        private static string StartupFile { get; } = Assembly.GetAssembly(typeof(ReferenceHub)).Location.Replace($"SCPSL_Data{Path.DirectorySeparatorChar}Managed{Path.DirectorySeparatorChar}Assembly-CSharp.dll", "SynapseStart-config.yml");
        private static string SynapseDirectory 
        { 
            get 
            {
                var Config = new YamlConfig(StartupFile);
                return Config.GetString("synapse_installation", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Synapse"));
            }
        }

        internal static string MainPluginDirectory { get; } = Path.Combine(SynapseDirectory, "Plugins");
        internal static string DependenciesDirectory { get; } = Path.Combine(SynapseDirectory, "dependencies");
        internal static string MainConfigDirectory { get; } = Path.Combine(SynapseDirectory, "ServerConfigs");
        private static string ServerPluginDirectory { get; set; }
        internal static string ServerConfigDirectory { get; private set; }
        private static string ServerConfigFile { get; set; }

        // ReSharper disable once NotAccessedField.Local
        private static Events.EventHandlers _eventHandler;

        // Methods
        internal static IEnumerator<float> StartSynapse()
        {
            yield return Timing.WaitForSeconds(0.5f);
            try
            {
                Files();

                LoadPlugins();

                LoadSynapse();
            }
            catch (Exception e)
            {
                Log.Error($"Synapse could not Start : {e}");
            }
        }

        private static void Files()
        {
            ServerPluginDirectory = Path.Combine(MainPluginDirectory, $"Server-{ServerStatic.ServerPort}");
            if (!Directory.Exists(ServerPluginDirectory))
                Directory.CreateDirectory(ServerPluginDirectory);

            ServerConfigDirectory = Path.Combine(MainConfigDirectory, $"Server-{ServerStatic.ServerPort}");
            if (!Directory.Exists(ServerConfigDirectory))
                Directory.CreateDirectory(ServerConfigDirectory);

            ServerConfigFile = Path.Combine(ServerConfigDirectory, "server-config.yml");
            if (!File.Exists(ServerConfigFile))
                File.Create(ServerConfigFile).Close();
        }

        private static void LoadPlugins()
        {
            LoadDependencies();

            var plugins = Directory.GetFiles(ServerPluginDirectory);

            foreach (var plugin in plugins)
            {
                if (plugin.EndsWith("Synapse.dll")) continue;

                if (plugin.EndsWith(".dll")) LoadPlugin(plugin);
            }
            Plugin.Config = new YamlConfig(ServerConfigFile);
            OnEnable();
        }

        private static void LoadSynapse()
        {
            SynapseConfigs.ReloadConfig();
            HarmonyPatch();
            _eventHandler = new Events.EventHandlers();
            
            ServerConsole.ReloadServerName();

            try
            {
                PermissionReader.Init();
            }
            catch (Exception e)
            {
                Log.Error($"Your Permission in invalid: {e}");
            }
        }


        private static void LoadDependencies()
        { 
            Log.Info("Loading Dependencies...");
            var depends = Directory.GetFiles(DependenciesDirectory);

            foreach (var dll in depends)
            {
                if (!dll.EndsWith(".dll")) continue;

                if (IsLoaded(dll))
                    return;

                var assembly = Assembly.LoadFrom(dll);
                LoadedDependencies.Add(assembly);
                Log.Info($"Successfully loaded {assembly.GetName().Name}");
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
                var file = ModLoader.ReadFile(pluginPath);
                var assembly = Assembly.Load(file);

                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsAbstract) continue;

                    if (type.FullName == null) continue;

                    if (!typeof(Plugin).IsAssignableFrom(type)) continue;

                    var plugin = Activator.CreateInstance(type);

                    if (!(plugin is Plugin p)) continue;

                    p.Details = type.GetCustomAttribute<PluginDetails>();
                    if (p.Details == null)
                        p.Details = new PluginDetails()
                        {
                            Author = "?",
                            Description = "?",
                            Name = "?",
                            Version = "?",
                        };

                    Plugins.Add(p);
                    Log.Info($"Successfully loaded {p.Details.Name}");
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
                try
                {
                    plugin.Translation = new Translation {Plugin = plugin};
                    plugin.OwnPluginFolder = Path.Combine(ServerPluginDirectory, plugin.Details.Name);
                    plugin.OnEnable();
                }
                catch (Exception e)
                {
                    Log.Error($"Plugin {plugin.Details.Name} threw an exception while enabling {e}");
                }
        }

        internal static void OnConfigReload()
        {
            Plugin.Config = new YamlConfig(ServerConfigFile);
            SynapseConfigs.ReloadConfig();

            foreach (var plugin in Plugins)
                try
                {
                    plugin.InvokeConfigReloadEvent();
                }
                catch (Exception e)
                {
                    Log.Error($"Plugin {plugin.Details.Name} threw an exception while reloading {e}");
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
    }
}