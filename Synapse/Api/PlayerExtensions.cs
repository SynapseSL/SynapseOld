using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Mirror;
using UnityEngine;

namespace Synapse.Api
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class PlayerExtensions
    {
        private static MethodInfo sendSpawnMessage;
        public static MethodInfo SendSpawnMessage
        {
            get
            {
                if (sendSpawnMessage == null)
                    sendSpawnMessage = typeof(NetworkServer).GetMethod("SendSpawnMessage",BindingFlags.Instance | BindingFlags.InvokeMethod
                        | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);

                return sendSpawnMessage;
            }
        }

        /// <summary>Gives a User a Message im Remote Admin</summary>
        /// <param name="sender">The User who you send the Message</param>
        /// <param name="pluginName">The Name from which is it at the beginning of the Message</param>
        /// <param name="message">The Message you want to send</param>
        /// <param name="success">True = green the command is right you have permission and execute it successfully</param>
        /// <param name="type">In Which Category should you see it too?</param>
        public static void RaMessage(this CommandSender sender, string message, bool success = true,
            RaCategory type = RaCategory.None)
        {
            var category = "";
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (type)
            {
                case RaCategory.None:
                    category = "";
                    break;
                case RaCategory.PlayerInfo:
                    category = "PlayerInfo";
                    break;
                case RaCategory.ServerEvents:
                    category = "ServerEvents";
                    break;
                case RaCategory.DoorsManagement:
                    category = "DoorsManagement";
                    break;
                case RaCategory.AdminTools:
                    category = "AdminTools";
                    break;
                case RaCategory.ServerConfigs:
                    category = "ServerConfigs";
                    break;
                case RaCategory.PlayersManagement:
                    category = "PlayersManagement";
                    break;
            }


            sender.RaReply($"{Assembly.GetCallingAssembly().GetName().Name}#" + message, success, true, category);
        }

        public static IEnumerable<Player> GetAllPlayers()
        {
            return (from gameobject in PlayerManager.players
                    where gameobject != PlayerManager.localPlayer && gameobject != null
                    select gameobject.GetPlayer()).ToList();
        }

        public static Player GetPlayer(this MonoBehaviour mono) => mono.GetComponent<Player>();

        public static Player GetPlayer(this GameObject gameObject) => gameObject.GetComponent<Player>();

        public static Player GetPlayer(int id) => GetAllPlayers().FirstOrDefault(p => p.PlayerId == id);

        public static Player GetPlayer(string arg)
        {
            if (short.TryParse(arg, out var playerId))
                return GetPlayer(playerId);

            if (!arg.EndsWith("@steam") && !arg.EndsWith("@discord") && !arg.EndsWith("@northwood") &&
                !arg.EndsWith("@patreon"))
                return GetAllPlayers().FirstOrDefault(p => p.NickName.ToLower().Contains(arg.ToLower()));
            foreach (var player in GetAllPlayers())
                if (player.UserID == arg)
                    return player;

            return GetAllPlayers().FirstOrDefault(p => p.NickName.ToLower().Contains(arg.ToLower()));
        }
    }
}