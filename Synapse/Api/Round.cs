using GameCore;
using Respawning;
using System;

namespace Synapse.Api
{
    public static class Round
    {
        [Obsolete("Please use RoundLength")]
        public static TimeSpan RoundLenght => RoundStart.RoundLenght;

        /// <summary>
        /// The time since the Round started
        /// </summary>
        public static TimeSpan RoundLength => RoundStart.RoundLenght;

        /// <summary>
        /// The time the Round started
        /// </summary>
        public static DateTime StartedTime => DateTime.Now - RoundLength;

        /// <summary>
        /// Is the Round started?
        /// </summary>
        public static bool IsStarted => RoundSummary.RoundInProgress();

        /// <summary>
        /// Get/Sets if the Round is locked
        /// </summary>
        public static bool IsLocked{ get => RoundSummary.RoundLock; set => RoundSummary.RoundLock = value; }

        /// <summary>
        /// Get/Sets if the Lobby is locked
        /// </summary>
        public static bool IsLobbyLocked { get => RoundStart.LobbyLock; set => RoundStart.LobbyLock = value; }

        /// <summary>
        /// The Amount of escaped ClassD`s
        /// </summary>
        public static int EscapedDs { get => RoundSummary.escaped_ds; set => RoundSummary.escaped_ds = value; }

        /// <summary>
        /// The Amount of escaped Scientists
        /// </summary>
        public static int EscapedScientists { get => RoundSummary.escaped_scientists; set => RoundSummary.escaped_scientists = value; }

        /// <summary>
        /// The Amount of kills by Scps
        /// </summary>
        public static int KillsByScps { get => RoundSummary.kills_by_scp; set => RoundSummary.kills_by_scp = value; }

        /// <summary>
        /// The Amount of persons changed into Zombies
        /// </summary>
        public static int ChangedIntoZombies { get => RoundSummary.changed_into_zombies; set => RoundSummary.changed_into_zombies = value; }

        /// <summary>
        /// Restarts the Round
        /// </summary>
        public static void Restart() => Player.Host.PlayerStats.Roundrestart();

        /// <summary>
        /// Starts the Round
        /// </summary>
        public static void Start() => CharacterClassManager.ForceRoundStart();

        /// <summary>
        /// Spawns Mtf/Chaos
        /// </summary>
        /// <param name="isCI"></param>
        public static void MtfRespawn(bool isCI = false)
        {
            var component = Server.Host.GetComponent<RespawnManager>();
            component.NextKnownTeam = isCI ? SpawnableTeamType.ChaosInsurgency : SpawnableTeamType.NineTailedFox;
            component.Spawn();
        }
    }
}
