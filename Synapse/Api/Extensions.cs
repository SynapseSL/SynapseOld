using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using CommandSystem;
using Harmony;
using Mirror;
using Synapse.Api.Enums;
using Synapse.Api.Plugin;
using UnityEngine;

namespace Synapse.Api
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class Extensions
    {
        /// <summary>Gives a player a message in the RemoteAdmin</summary>
        /// <param name="sender">The User who you send the Message</param>
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

        public static CommandSender CommandSender(this ICommandSender sender) => sender as CommandSender;

        public static Player GetPlayer(this ICommandSender sender)
        {
            return sender.CommandSender().SenderId == "SERVER CONSOLE" || sender.CommandSender().SenderId == "GAME CONSOLE"
            ? Player.Host
            : Player.GetPlayer(sender.CommandSender().SenderId);
        }

        /// <summary>
        /// Gives all players on the server with this Role
        /// </summary>
        /// <param name="role"></param>
        public static IEnumerable<Player> GetAllPlayers(this RoleType role) => Player.GetAllPlayers().Where(x => x.Role == role);


        public static IEnumerable<Player> GetAllPlayers(this RoleType[] roles) => Player.GetAllPlayers().Where(x => roles.Any(r => x.Role == r));

        public static IEnumerable<Player> GetAllPlayers(this Team[] teams) => Player.GetAllPlayers().Where(x => teams.Any(t => x.Team == t));

        /// <summary>
        /// Gives all players on the server in this Team
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public static IEnumerable<Player> GetAllPlayers(this Team team) => Player.GetAllPlayers().Where(x => x.Team == team);

        /// <summary>
        /// Gives you the player object
        /// </summary>
        public static Player GetPlayer(this MonoBehaviour mono) => mono.GetComponent<Player>();

        /// <summary>
        /// Gives you the player object
        /// </summary>
        public static Player GetPlayer(this PlayableScps.PlayableScp scp) => scp.Hub.GetPlayer();

        /// <summary>
        /// Gives you the player object
        /// </summary>
        public static Player GetPlayer(this GameObject gameObject) => gameObject.GetComponent<Player>();

        public static string GetVersionString(this PluginDetails details) => $"{details.SynapseMajor}.{details.SynapseMinor}.{details.SynapsePatch}";

        public static int GetVersionNumber(this PluginDetails details) => details.SynapseMajor * 100 + details.SynapseMinor * 10 + details.SynapsePatch;
    }
}