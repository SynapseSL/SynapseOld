using GameCore;
using System;

namespace Synapse.Api
{
    public static class Round
    {
        public static TimeSpan RoundLenght => RoundStart.RoundLenght;

        public static DateTime StartedTime => DateTime.Now - RoundLenght;

        public static bool IsStarted => RoundSummary.RoundInProgress();

        /// <summary>
        /// Activates/Deactivates the RoundLock (if the Round can end)
        /// </summary>
        public static bool IsLocked{ get => RoundSummary.RoundLock; set => RoundSummary.RoundLock = value; }

        public static int Escaped_Ds { get => RoundSummary.escaped_ds; set => RoundSummary.escaped_ds = value; }

        public static int Escaped_Scientists { get => RoundSummary.escaped_scientists; set => RoundSummary.escaped_scientists = value; }

        public static int Kills_by_Scps { get => RoundSummary.kills_by_scp; set => RoundSummary.kills_by_scp = value; }

        public static int Changed_into_Zombies { get => RoundSummary.changed_into_zombies; set => RoundSummary.changed_into_zombies = value; }

        /// <summary>
        /// Activates/Deactivates the LobbyLock (if the Lobby can continue counting down)
        /// </summary>
        public static bool IsLobbyLocked{ get => GameCore.RoundStart.LobbyLock; set => GameCore.RoundStart.LobbyLock = value; }

        public static void Restart() => Player.Host.PlayerStats.Roundrestart();

        public static void Start() => CharacterClassManager.ForceRoundStart();
    }
}
