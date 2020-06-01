using Assets._Scripts.Dissonance;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Synapse.Events
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class PlayerJoinEvent : EventArgs
    {
        // Properties
        public ReferenceHub Player { get; private set; }

        public string Nick { get; set; }

        // Constructor
        public PlayerJoinEvent(ReferenceHub player)
        {
            Player = player;
        }
    }

    public class RemoteCommandEvent : EventArgs
    {
        public CommandSender Sender { get; internal set; }

        public ReferenceHub Player
        {
            get => Sender.SenderId == "SERVER CONSOLE" || Sender.SenderId == "GAME CONSOLE" ? ReferenceHub.GetHub(PlayerManager.localPlayer) : Api.Player.GetPlayer(Sender.SenderId);
        }

        public bool Allow { get; set; }

        public string Command { get; internal set; }
    }

    public class SpeakEvent : EventArgs
    {
        public ReferenceHub Player { get; internal set; }

        public DissonanceUserSetup DissonanceUserSetup { get; internal set; }

        public bool Scp939Talk { get; set; }

        public bool IntercomTalk { get; set; }

        public bool RadioTalk { get; set; }

        public bool ScpChat { get; set; }

        public bool SpectatorChat { get; set; }
    }

    public class Scp049RecallEvent : EventArgs
    {
        public ReferenceHub Player { get; internal set; }

        public ReferenceHub Target { get; set; }

        public bool Allow { get; set; }

        public RoleType RespawnRole { get; set; }

        public float TargetHealth { get; set; }

        public Ragdoll Ragdoll { get; set; }
    }

    public class ConsoleCommandEvent
    {
        public ReferenceHub Player { get; internal set; }

        public string Command { get; internal set; }

        public string ReturnMessage { get; set; }

        public string Color { get; set; }
    }

    public class PlayerLeaveEvent
    {
        public ReferenceHub Player { get; internal set; }
    }

    public class PlayerBanEvent
    {
        public ReferenceHub BannedPlayer { get; internal set; }

        public bool Allowed { get; set; } = true;
        
        public string UserId { get; internal set; }
        
        public int Duration { get; internal set; }
        
        public ReferenceHub Issuer { get; internal set; }
        
        public string Reason { get; internal set; }
    }
}
