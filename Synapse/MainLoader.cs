using System.Diagnostics.CodeAnalysis;
using System.IO;
using MEC;

namespace Synapse
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class MainLoader
    {
        public const string version = "v.1.4-Beta";
        /// <summary>This method is called by the Scp Server in the assembly-csharp</summary>
        // ReSharper disable once UnusedMember.Global
        public static void LoaderExecutionCode()
        {
            Log.Info($"Now starting Synapse Version {version}");
            Log.Info("Created by Dimenzio and SirRoob");

            Log.Info("Checking Files");

            if (!Directory.Exists(SynapseManager.MainConfigDirectory))
                Directory.CreateDirectory(SynapseManager.MainConfigDirectory);

            if (!Directory.Exists(SynapseManager.MainPluginDirectory))
                Directory.CreateDirectory(SynapseManager.MainPluginDirectory);

            if (!Directory.Exists(SynapseManager.DependenciesDirectory))
                Directory.CreateDirectory(SynapseManager.DependenciesDirectory);

            CustomNetworkManager.Modded = true;
            try
            {
                Timing.RunCoroutine(SynapseManager.StartSynapse());
            }
            catch
            {
                Log.Error("PluginManager failed to Start restart the Server");
            }
        }
    }
}