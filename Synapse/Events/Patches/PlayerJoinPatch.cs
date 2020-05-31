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
                var player = __instance.GetComponent<ReferenceHub>();

                if (!string.IsNullOrEmpty(player.characterClassManager.UserId))
                    Events.InvokePlayerJoinEvent(player, ref n);
            }
            catch (Exception e)
            {
                Log.Error($"PlayerJoin Event Error: {e}");
            }
            return true;
        }
    }
}
