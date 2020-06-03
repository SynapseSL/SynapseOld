using System;
using Harmony;

namespace Synapse.Events.Patches
{
    public class PatchHandler
    {
        // Variables
        private HarmonyInstance _instance;
        private int _patchFixer;

        // Methods
        public void PatchMethods()
        {
            Log.Info("Harmony Patching started!");
            try
            {
                _patchFixer++;
                _instance = HarmonyInstance.Create($"SynapseEvents.patches.{_patchFixer}");
                _instance.PatchAll();
                Log.Info("Harmony Patching Done!");
            }
            catch (Exception e)
            {
                Log.Error($"Harmony Event Patch Error: {e}");
            }
        }

        // ReSharper disable once UnusedMember.Global
        public void UnPatchMethods()
        {
            _instance.UnpatchAll();
        }
    }
}