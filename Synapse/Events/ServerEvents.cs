using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Synapse.Events.Classes;

namespace Synapse.Events
{
    [SuppressMessage("ReSharper", "EventNeverSubscribedTo.Global")]
    public static partial class Events
    {
        //ConsoleCommandEvent
        public delegate void OnConsoleCommand(ref ConsoleCommandEvent ev);


        //RemoteCommandEvent
        public delegate void OnRemoteCommand(ref RemoteCommandEvent ev);

        //RoundEndEvent
        public delegate void OnRoundEnd();

        // RoundRestartEvent
        public delegate void OnRoundRestart();


        // RoundStartEvent
        public delegate void OnRoundStart();

        //TeamRespawnEvent
        public delegate void TeamRespawn(ref TeamRespawnEvent ev);

        public static event OnRoundRestart RoundRestartEvent;

        internal static void InvokeRoundRestart()
        {
            RoundRestartEvent?.Invoke();
        }

        /// <summary>
        ///     A Event which activate when the Round Ends (not a Restart!)
        /// </summary>
        public static event OnRoundEnd RoundEndEvent;

        internal static void InvokeRoundEndEvent()
        {
            RoundEndEvent?.Invoke();
        }

        public static event OnRoundStart RoundStartEvent;

        internal static void InvokeRoundStart()
        {
            RoundStartEvent?.Invoke();
        }

        /// <summary>A Event which is activated when a user send a Command in the Remote Admin</summary>
        /// <remarks>It need to hook ref RemoteCommandEvent ev</remarks>
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

            RemoteCommandEvent.Invoke(ref ev);

            allow = ev.Allow;
        }

        /// <summary>A Event which is activated when a user send a Command in the Remote Admin</summary>
        public static event OnConsoleCommand ConsoleCommandEvent;

        internal static void InvokeConsoleCommandEvent(ReferenceHub player, string command, out string color,
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

            ConsoleCommandEvent.Invoke(ref ev);

            color = ev.Color;
            returning = ev.ReturnMessage;
        }

        public static event TeamRespawn TeamRespawnEvent;
        
        internal static void InvokeTeamRespawnEvent(ref List<ReferenceHub> respawnlist, ref bool ischaos, ref bool allow ,ref bool useticktes)
        {
            if (TeamRespawnEvent == null) return;

            var ev = new TeamRespawnEvent()
            {
                Allow = allow,
                IsChaos = ischaos,
                RespawnList = respawnlist,
                UseTickets = useticktes
            };

            TeamRespawnEvent.Invoke(ref ev);

            respawnlist = ev.RespawnList;
            ischaos = ev.IsChaos;
            allow = ev.Allow;
            useticktes = ev.UseTickets;
        }
    }
}