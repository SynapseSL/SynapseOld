using Synapse.Events.Classes;

namespace Synapse.Events
{
    public static partial class Events
    {
        public delegate void OnWaitingForPlayers();
        public static event OnWaitingForPlayers WaitingForPlayersEvent;
        internal static void InvokeWaitingForPlayers() => WaitingForPlayersEvent?.Invoke();

        public delegate void OnCheckRoundEnd(CheckRoundEndEvent ev);
        public static event OnCheckRoundEnd CheckRoundEndEvent;
        public static void InvokeCheckRoundEnd(ref bool forceEnd, ref bool allow, ref RoundSummary.LeadingTeam team,
            ref bool teamChanged)
        {
            var ev = new CheckRoundEndEvent
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
    }
}
