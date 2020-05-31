using System;
using Harmony;

namespace Synapse.Events.Patches
{
    public class PatchHandler
    {
        //Variablen
        private HarmonyInstance instance;
        private int patchFixer = 0;

        //Methoden
        public void PatchMethods()
        {
            Log.Info("Harmony Patching started!");
            try
            {
                patchFixer++;
                instance = HarmonyInstance.Create($"SynapsEvents.patches.{patchFixer}");
                instance.PatchAll();
                Log.Info("Harmony Patching Done!");
            }
            catch (Exception e)
            {
                Log.Error($"Harmony Event Patch Error: {e}");
            }
        }

        public void UnPatchMethods()
        {
            instance.UnpatchAll();
        }
    }
}
