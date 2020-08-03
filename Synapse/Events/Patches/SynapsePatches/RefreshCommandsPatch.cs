using CommandSystem.Commands;
using Harmony;

namespace Synapse.Events.Patches.SynapsePatches
{
    [HarmonyPatch(typeof(RefreshCommandsCommand), nameof(RefreshCommandsCommand.Execute)]
    static class RefreshCommandsPatch
    {
        public static bool Prefix(ref string respone)
        {
            respone = "Synapse doenst allow to refresh the commands!";
            return false;
        }
    }
}
