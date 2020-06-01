using Synapse.Events.Classes;

namespace Synapse.Events
{
    public static partial class Events
    {
        // RoundRestartEvent
        public delegate void OnRoundRestart();
        public static event OnRoundRestart RoundRestartEvent;
        internal static void InvokeRoundRestart() => RoundRestartEvent?.Invoke();
        
        //RoundEndEvent
        public delegate void OnRoundEnd();
        /// <summary>
        /// A Event which activate when the Round Ends (not a Restart!)
        /// </summary>
        public static event OnRoundEnd RoundEndEvent;
        internal static void InvokeRoundEndEvent() => RoundEndEvent?.Invoke();
        
        
        // RoundStartEvent
        public delegate void OnRoundStart();
        public static event OnRoundStart RoundStartEvent;
        internal static void InvokeRoundStart() => RoundStartEvent?.Invoke();
        

        //RemoteCommandEvent
        public delegate void OnRemoteCommand(ref RemoteCommandClass ev);
        /// <summary>A Event which is activated when a user send a Command in the Remote Admin</summary>
        /// <remarks>It need to hook ref RemoteCommandEvent ev</remarks>
        public static event OnRemoteCommand RemoteCommandEvent;
        internal static void InvokeRemoteCommandEvent(CommandSender sender, string command, ref bool allow)
        {
            if (RemoteCommandEvent == null) return;

            var ev = new RemoteCommandClass()
            {
                Allow = allow,
                Sender = sender,
                Command = command,
            };

            RemoteCommandEvent.Invoke(ref ev);

            allow = ev.Allow;
        }


        //ConsoleCommandEvent
        public delegate void OnConsoleCommand(ref ConsoleCommandClass ev);
        /// <summary>A Event which is activated when a user send a Command in the Remote Admin</summary>
        public static event OnConsoleCommand ConsoleCommandEvent;
        internal static void InvokeConsoleCommandEvent(ReferenceHub player, string command, out string color, out string returning)
        {
            color = "red";
            returning = "";
            if (ConsoleCommandEvent == null) return;

            var ev = new ConsoleCommandClass()
            {
                Command = command,
                Player = player,
            };

            ConsoleCommandEvent.Invoke(ref ev);

            color = ev.Color;
            returning = ev.ReturnMessage;
        }
    }
}