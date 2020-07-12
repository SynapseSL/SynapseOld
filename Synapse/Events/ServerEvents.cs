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
        public delegate void OnRoundRestart();
        public static event OnRoundRestart RoundRestartEvent;

        internal static void InvokeRoundRestart()
        {
            RoundRestartEvent?.Invoke();
        }

        /// <summary>
        ///     A Event which activate when the Round Ends (not a Restart!)
        /// </summary>
        public delegate void OnRoundEnd();
        public static event OnRoundEnd RoundEndEvent;

        internal static void InvokeRoundEndEvent()
        {
            RoundEndEvent?.Invoke();
        }

        public delegate void OnRoundStart();
        public static event OnRoundStart RoundStartEvent;

        internal static void InvokeRoundStart()
        {
            RoundStartEvent?.Invoke();
        }

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

        internal static void InvokeConsoleCommandEvent(Player player, string command, out string color,
            out string returning)
        {
            color = "red";
            returning = "";
            if (ConsoleCommandEvent == null) return;

            var ev = new ConsoleCommandEvent
            {
                Command = command,
                Player = player
            };

            ConsoleCommandEvent.Invoke(ev);

            color = ev.Color;
            returning = ev.ReturnMessage;
        }

        public delegate void TeamRespawn(TeamRespawnEvent ev);
        public static event TeamRespawn TeamRespawnEvent;
        
        internal static void InvokeTeamRespawnEvent(ref List<Player> respawnList, ref bool isChaos, ref bool allow ,ref bool useTickets)
        {
            if (TeamRespawnEvent == null) return;

            var ev = new TeamRespawnEvent
            {
                Allow = allow,
                IsChaos = isChaos,
                RespawnList = respawnList,
                UseTickets = useTickets
            };

            TeamRespawnEvent.Invoke(ev);

            respawnList = ev.RespawnList;
            isChaos = ev.IsChaos;
            allow = ev.Allow;
            useTickets = ev.UseTickets;
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

        public delegate void OnWaitingForPlayers();
        public static event OnWaitingForPlayers WaitingForPlayersEvent;
        internal static void InvokeWaitingForPlayers() => WaitingForPlayersEvent?.Invoke();

        public delegate void OnCheckRoundEnd(CheckRoundEndEvent ev);
        public static event OnCheckRoundEnd CheckRoundEndEvent;

        public static void InvokeCheckRoundEnd(ref bool forceEnd, ref bool allow, ref RoundSummary.LeadingTeam team,
            ref bool teamChanged)
        {
            var ev = new CheckRoundEndEvent()
            {
                Allow = allow,
                ForceEnd = forceEnd,
                LeadingTeam = team
            };
            
            CheckRoundEndEvent?.Invoke(ev);

            teamChanged = team != ev.LeadingTeam;
            team = ev.LeadingTeam;
            allow = ev.Allow;
            forceEnd = ev.ForceEnd;
        }
    }
}