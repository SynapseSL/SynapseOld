using System;
using Synapse.Api;
using GameCore;
using Harmony;
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
                var banIssuer = Player.GetPlayer(issuer);
                var allow = true;
                Events.InvokePlayerBanEvent(player, duration, ref allow, reason, banIssuer);

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