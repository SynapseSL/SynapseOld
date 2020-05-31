using System.Diagnostics.CodeAnalysis;
using MEC;
using System.IO;

namespace Synapse
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class MainLoader
    {
        /// <summary>This method is called by the Scp Server in the assembly-csharp</summary>
        // ReSharper disable once UnusedMember.Global
        public static void LoaderExecutionCode()
        {
            Log.Info("Checking Files");

            if (!Directory.Exists(PluginManager.ConfigDirectory))
                Directory.CreateDirectory(PluginManager.ConfigDirectory);

            if (!Directory.Exists(PluginManager.MainPluginDirectory))
                Directory.CreateDirectory(PluginManager.MainPluginDirectory);

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