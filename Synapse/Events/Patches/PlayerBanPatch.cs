using System;
using GameCore;
using Harmony;
using Synapse.Api;
using UnityEngine;

namespace Synapse.Events.Patches
{
    [HarmonyPatch(typeof(BanPlayer), nameof(BanPlayer.BanUser), typeof(GameObject), typeof(int), typeof(string),
        typeof(string), typeof(bool))]
    public class PlayerBanPatch
    {
        public static bool Prefix(GameObject user, int duration, string reason, string issuer, bool isGlobalBan)
        {
            try
            {
                var player = user.GetPlayer();
                var banIssuer = PlayerExtensions.GetPlayer(issuer);
                var allow = true;
                Events.InvokePlayerBanEvent(player, player.UserID, duration, ref allow, reason, banIssuer);

                return isGlobalBan && ConfigFile.ServerConfig.GetBool("gban_ban_ip") || allow;
            }
            catch (Exception e)
            {
                Log.Error($"BanEvent Error: {e}");
                return true;
            }
        }
    }
}