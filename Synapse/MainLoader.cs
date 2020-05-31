using MEC;
using System.IO;

namespace Synapse
{
    public class MainLoader
    {
        /// <summary>This method is called by the Scp Server in the assembly-csharp</summary>
        public static void LoaderExecutionCode()
        {
            Log.Info("Checking Files");

            if (!Directory.Exists(PluginManager.ConfigDirectory))
                Directory.CreateDirectory(PluginManager.ConfigDirectory);

            if (!File.Exists(PluginManager.ConfigPath))
                File.Create(PluginManager.ConfigPath).Close();

            if (!Directory.Exists(PluginManager.MainPluginDirectoty))
                Directory.CreateDirectory(PluginManager.MainPluginDirectoty);

            if (!Directory.Exists(PluginManager.ServerPluginDirectory))
                Directory.CreateDirectory(PluginManager.ServerPluginDirectory);

            if (!Directory.Exists(PluginManager.DependenciesDirectory))
                Directory.CreateDirectory(PluginManager.DependenciesDirectory);

            CustomNetworkManager.Modded = true;
            try
            {
                Timing.RunCoroutine(PluginManager.LoadPlugins());
            }
            catch
            {
                Log.Error("PluginManager failed to Start restart the Server");
            }
        }
    }
}