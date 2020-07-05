using System;
using Harmony;
using Synapse.Api;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(NicknameSync), nameof(NicknameSync.UpdateNickname))]
    public static class PlayerJoinPatch
    {
        // ReSharper disable once InconsistentNaming
        public static bool Prefix(NicknameSync __instance, ref string n)
        {
            try
            {
                var player = __instance.GetPlayer();

                if (!string.IsNullOrEmpty(player.UserId))
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