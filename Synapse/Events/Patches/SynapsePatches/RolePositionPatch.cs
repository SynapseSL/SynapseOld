using Harmony;

namespace Synapse.Events.Patches.SynapsePatches
{
    [HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.SetClassID))]
    public static class RolePositionPatch
    {
        internal static bool Lite = false;

        public static bool Prefix(CharacterClassManager __instance, RoleType id)
        {
            __instance.SetClassIDAdv(id, Lite, false);
            Lite = false;
            return false;
        }
    }
}
