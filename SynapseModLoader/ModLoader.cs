using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SynapseModLoader
{
    // ReSharper disable once UnusedType.Global
    
    /// <summary>
    /// This Class is injected directly into the Assembly-CSharp and calls the MainLoader of Synapse.
    /// The Injection and Call happens not directly at Boot Time but when the ServerConsole gets constructed.
    /// </summary>
    public class ModLoader
    {
        private static byte[] ReadFile(string path)
        {
            var fileStream = File.Open(path, FileMode.Open);
            byte[] result;
            using (var memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);
                result = memoryStream.ToArray();
            }
            fileStream.Close();
            return result;
        }
        
        // ReSharper disable once UnusedMember.Global
        public static void LoadModSystem()
        {
            if (_loaded)
            {
                return;
            }
            ServerConsole.AddLog("Synapse Mod-Loader is now initialising.. :)", ConsoleColor.Blue);

            try
            {
                var startupFile = Assembly.GetExecutingAssembly().Location.Replace($"SCPSL_Data{Path.DirectorySeparatorChar}Managed{Path.DirectorySeparatorChar}Assembly-CSharp.dll", "SynapseStart-config.yml");
                if (!File.Exists(startupFile))
                {
                    ServerConsole.AddLog($"Synapse Mod-Loader Start file is missing ... creating: {startupFile}", ConsoleColor.Blue);
                    File.Create(startupFile).Close();
                    File.WriteAllLines(startupFile, new [] { "synapse_installation: default" });
                }
                var config = new YamlConfig(startupFile);

                var text = config.GetString("synapse_installation", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Synapse"));
                if (!Directory.Exists(text))
                {
                    Directory.CreateDirectory(text);
                }
                if (!File.Exists(Path.Combine(text, "Synapse.dll")))
                {
                    ServerConsole.AddLog("Error while loading Synapse! The Synapse.dll is missing!", ConsoleColor.Red);
                    return;
                }
                var methodInfo = Assembly.Load(ReadFile(Path.Combine(text, "Synapse.dll"))).GetTypes()
                    .SelectMany(p => p.GetMethods()).FirstOrDefault(f => f.Name == "LoaderExecutionCode");
                if (!(methodInfo != null))
                {
                    return;
                }
                methodInfo.Invoke(null, null);
                _loaded = true;
            }
            catch (Exception e)
            {
                ServerConsole.AddLog($"Synapse Mod-Loader startup Error: {e}", ConsoleColor.Red);
            }
        }

        private static bool _loaded;
    }
}