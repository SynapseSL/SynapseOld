using MEC;
using Synapse.Api;
using Synapse.Api.Plugin;
using Synapse.Config;
using Synapse.Events;
using Synapse.Events.Patches;
using SynapseModLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Synapse
{
    public static class Synapse
    {
        #region Version
        private const int MajorVersion = 1;
        private const int MinorVerion = 0;
        private const int Patch = 0;

        public static int VersionNumber => MajorVersion * 100 + MinorVerion * 10 + Patch;
        public static string Version => $"{MajorVersion}.{MinorVerion}.{Patch}";
        #endregion

        private static bool isLoaded = false;
        private static readonly List<Assembly> LoadedDependencies = new List<Assembly>();
        internal static readonly List<Plugin> plugins = new List<Plugin>(); //TODO: Rework Config System and make this private
        private static EventHandlers _eventHandler;
        public static List<PluginDetails> Plugins => plugins.Select(obj => obj.Details).ToList();


        public static void LoaderExecutionCode()
        {
            if (isLoaded) return;

            Log.Info($"Now starting Synapse Version {Synapse.Version}");
            Log.Info("Created by Dimenzio and SirRoob");

            CustomNetworkManager.Modded = true;

            try
            {
                Timing.CallDelayed(0.5f, () => Synapse.Start());
                isLoaded = true;
            }
            catch
            {
                Log.Error("Synapse failed to Start.Restart the Server");
            }
        }
        internal static void Start()
        {
            LoadDependencies();

            foreach (var plugin in Directory.GetFiles(Files.ServerPluginDirectory))
            {
                if (plugin == "Synapse.dll") continue;

                if (plugin.EndsWith(".dll")) LoadPlugin(plugin);
            }


            ConfigManager.InitializeConfigs();
            HarmonyPatch();
            ServerConsole.ReloadServerName();
            _eventHandler = new Events.EventHandlers();
            try
            {
                PermissionReader.Init();
            }
            catch (Exception e)
            {
                Log.Error($"Your Permission in invalid: {e}");
            }

            OnEnable();
        }


        #region Methods to do everything what Synapse needs
        private static void LoadDependencies()
        {
            Log.Info("Loading Dependencies...");
            var depends = Directory.GetFiles(Files.DependenciesDirectory);

            foreach (var dll in depends)
            {
                if (!dll.EndsWith(".dll")) continue;

                if (LoadedDependencies.Any(x => x.Location == dll))
                    return;

                var assembly = Assembly.LoadFrom(dll);
                LoadedDependencies.Add(assembly);
                Log.Info($"Successfully loaded {assembly.GetName().Name}");
            }
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
                            Author = "Unknown",
                            Description = "No Description",
                            Name = assembly.GetName().Name,
                            Version = assembly.ImageRuntimeVersion,
                            SynapseMajor = MajorVersion,
                            SynapseMinor = MinorVerion,
                            SynapsePatch = Patch
                        };

                    plugins.Add(p);
                    if (p.Details.GetVersionNumber() == VersionNumber) Log.Info($"Successfully loaded {p.Details.Name}");

                    else if (p.Details.GetVersionNumber() > VersionNumber) Log.Warn($"The Plugin {p.Details.Name} is for the newer Synapse version {p.Details.GetVersionString()} but was succesfully loaded(bugs can occure)");
                    
                    else Log.Warn($"The Plugin {p.Details.Name} is for the older Synapse version {p.Details.GetVersionString()} but was succesfully loaded(bugs can occure)");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Error while initializing {pluginPath}: {e}");
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
        private static void OnEnable()
        {
            foreach (var plugin in plugins)
                try
                {
                    plugin.Translation = new Translation { Plugin = plugin };
                    plugin.OwnPluginFolder = Path.Combine(Files.ServerPluginDirectory, plugin.Details.Name);
                    plugin.OnEnable();
                }
                catch (Exception e)
                {
                    Log.Error($"Plugin {plugin.Details.Name} threw an exception while enabling {e}");
                }
        }
        #endregion
    }

    internal static class Files
    {
        //Synapse Directory
        private static string StartupFile => Assembly.GetAssembly(typeof(ReferenceHub)).Location.Replace($"SCPSL_Data{Path.DirectorySeparatorChar}Managed{Path.DirectorySeparatorChar}Assembly-CSharp.dll", "SynapseStart-config.yml");
        internal static string SynapseDirectory => new YamlConfig(StartupFile).GetString("synapse_installation", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Synapse"));

        //Directory-Structure
        internal static string DependenciesDirectory
        {
            get
            {
                var path = Path.Combine(SynapseDirectory, "dependencies");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                return path;
            }
        }
        internal static string MainPluginDirectory
        {
            get
            {
                var path = Path.Combine(SynapseDirectory, "Plugins");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                return path;
            }
        }
        internal static string MainConfigDirectory
        {
            get
            {
                var path = Path.Combine(SynapseDirectory, "ServerConfigs");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                return path;
            }
        }
        internal static string ServerPluginDirectory
        {
            get
            {
                var path = Path.Combine(MainPluginDirectory, $"Server-{ServerStatic.ServerPort}");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                return path;
            }
        }
        internal static string ServerConfigDirectory
        {
            get
            {
                var path = Path.Combine(MainConfigDirectory, $"Server-{ServerStatic.ServerPort}");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                return path;
            }
        }

        //Files
        internal static string ServerConfigFile
        {
            get
            {
                var path = Path.Combine(ServerConfigDirectory, "server-config.yml");

                if (!File.Exists(path))
                    File.Create(path).Close();

                return path;
            }
        }
        internal static string PermissionFile
        {
            get
            {
                var path = Path.Combine(Files.ServerConfigDirectory, "permissions.yml");

                if (!File.Exists(path))
                    File.WriteAllText(path, "groups:\n    user:\n        default: true\n        permissions:\n        - plugin.permission\n    northwood:\n        northwood: true\n        permissions:\n        - plugin.permission\n    owner:\n        permissions:\n        - .*");

                return path;
            }
        }
    }
}
