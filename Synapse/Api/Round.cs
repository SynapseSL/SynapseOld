using GameCore;
using Respawning;
using System;

namespace Synapse.Api
{
    public static class Round
    {
        [Obsolete("Please use RoundLength")]
        public static TimeSpan RoundLenght => RoundStart.RoundLenght;

        public static TimeSpan RoundLength => RoundStart.RoundLenght;

        public static DateTime StartedTime => DateTime.Now - RoundLength;

        public static bool IsStarted => RoundSummary.RoundInProgress();

        /// <summary>
        /// Activates/Deactivates the RoundLock (if the Round can end)
        /// </summary>
        public static bool IsLocked{ get => RoundSummary.RoundLock; set => RoundSummary.RoundLock = value; }

           /// <summary>
        /// Gets or sets the amount of Respawntickets for the MTF-Team
        /// Please be careful when settings the amount of the ticket since this method ignores whether or
        /// not the ticket amount should be locked to zero or not 
        /// </summary>
        public static int MtfTickets
        {
            get { 
                RespawnTickets.Singleton._tickets.TryGetValue(SpawnableTeamType.NineTailedFox, out var tickets); 
                return tickets; 
            }

            set => RespawnTickets.Singleton._tickets[SpawnableTeamType.NineTailedFox] = value;
        }
        
        /// <summary>
        /// Gets or sets the amount of Respawntickets for the Chaos-Team
        /// Please be careful when settings the amount of the ticket since this method ignores whether or
        /// not the ticket amount should be locked to zero or not 
        /// </summary>
        public static int ChaosTickets
        {
            get { 
                RespawnTickets.Singleton._tickets.TryGetValue(SpawnableTeamType.ChaosInsurgency, out var tickets); 
                return tickets; 
            }

            set => RespawnTickets.Singleton._tickets[SpawnableTeamType.ChaosInsurgency] = value;
        }

        /// <summary>
        /// Grants Respawntickets to the MTF-Team 
        /// </summary>
        /// <param name="tickets"></param> The amount of tickets granted
        /// <param name="overrideLocks"></param> Whether or not a existing lock should be ignored
        public static void GrantMtfTickets(int tickets, bool overrideLocks = false)
        {
            RespawnTickets.Singleton.GrantTickets(SpawnableTeamType.NineTailedFox, tickets, overrideLocks);
        }
        
        /// <summary>
        /// Grants Respawntickets to the Chaos-Team 
        /// </summary>
        /// <param name="tickets"></param> The amount of tickets granted
        /// <param name="overrideLocks"></param> Whether or not a existing lock should be ignored
        public static void GrantChaosTickets(int tickets, bool overrideLocks = false)
        {
            RespawnTickets.Singleton.GrantTickets(SpawnableTeamType.ChaosInsurgency, tickets, overrideLocks);
        }
        
        public static int EscapedDs { get => RoundSummary.escaped_ds; set => RoundSummary.escaped_ds = value; }

        public static int EscapedScientists { get => RoundSummary.escaped_scientists; set => RoundSummary.escaped_scientists = value; }

        public static int KillsByScps { get => RoundSummary.kills_by_scp; set => RoundSummary.kills_by_scp = value; }

        public static int ChangedIntoZombies { get => RoundSummary.changed_into_zombies; set => RoundSummary.changed_into_zombies = value; }

        /// <summary>
        /// Activates/Deactivates the LobbyLock (if the Lobby can continue counting down)
        /// </summary>
        public static bool IsLobbyLocked{ get => RoundStart.LobbyLock; set => RoundStart.LobbyLock = value; }

        public static void Restart() => Player.Host.PlayerStats.Roundrestart();

        public static void Start() => CharacterClassManager.ForceRoundStart();

        public static void MtfRespawn(bool isCI = false)
        {
            var component = Server.Host.GetComponent<RespawnManager>();
            component.NextKnownTeam = isCI ? SpawnableTeamType.ChaosInsurgency : SpawnableTeamType.NineTailedFox;
            component.Spawn();
        }
    }
}
