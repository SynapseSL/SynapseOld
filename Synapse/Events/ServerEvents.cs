using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LiteNetLib;
using Synapse.Api;
using Synapse.Events.Classes;

namespace Synapse.Events
{
    [SuppressMessage("ReSharper", "EventNeverSubscribedTo.Global")]
    public static partial class Events
    {
        /// <summary>A Event which is activated when a user send a Command in the Remote Admin</summary>
        /// <remarks>It need to hook ref RemoteCommandEvent ev</remarks>
        public delegate void OnRemoteCommand(RemoteCommandEvent ev);
        public static event OnRemoteCommand RemoteCommandEvent;
        internal static void InvokeRemoteCommandEvent(CommandSender sender, string command, ref bool allow)
        {
            if (RemoteCommandEvent == null) return;

            var ev = new RemoteCommandEvent
            {
                Allow = allow,
                Sender = sender,
                Command = command
            };

            RemoteCommandEvent.Invoke(ev);

            allow = ev.Allow;
        }

        /// <summary>A Event which is activated when a user send a Command in the Remote Admin</summary>
        public delegate void OnConsoleCommand(ConsoleCommandEvent ev);
        public static event OnConsoleCommand ConsoleCommandEvent;
        internal static void InvokeConsoleCommandEvent(Player player, string command, out bool allow)
        {
            allow = true;
            if (ConsoleCommandEvent == null) return;

            var ev = new ConsoleCommandEvent
            {
                Command = command,
                Player = player
            };

            ConsoleCommandEvent.Invoke(ev);
        }

        public delegate void TeamRespawn(TeamRespawnEvent ev);
        public static event TeamRespawn TeamRespawnEvent;
        internal static void InvokeTeamRespawnEvent(ref List<Player> respawnList, ref Respawning.SpawnableTeamType team)
        {
            if (TeamRespawnEvent == null) return;

            var ev = new TeamRespawnEvent
            {
                RespawnList = respawnList,
                Team = team
            };

            TeamRespawnEvent.Invoke(ev);

            team = ev.Team;
            respawnList = ev.RespawnList;
        }
        
        public delegate void OnPreAuthenticationEvent(PreAuthenticationEvent ev);
        public static event OnPreAuthenticationEvent PreAuthenticationEvent;
        internal static void InvokePreAuthentication(string userId, ConnectionRequest request, ref bool allow)
        {
            var ev = new PreAuthenticationEvent
            {
               Allow = allow,
               Request = request,
               UserId = userId
            };
            
            PreAuthenticationEvent?.Invoke(ev);

            allow = ev.Allow;
        }
    }
}