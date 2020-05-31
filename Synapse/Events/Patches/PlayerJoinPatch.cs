using System;
using Harmony;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(NicknameSync), nameof(NicknameSync.UpdateNickname))]
    public static class PlayerJoinPatch
    {
        public static bool Prefix(NicknameSync __instance, ref string n)
        {
            try
            {
                ReferenceHub Player = __instance.GetComponent<ReferenceHub>();

                if (!string.IsNullOrEmpty(Player.characterClassManager.UserId))
                    Events.InvokePlayerjoinEvent(Player, ref n);
            }
            catch (Exception e)
            {
                Log.Error($"PlayerJoin Event Error: {e}");
            }
            return true;
        }
    }
}
