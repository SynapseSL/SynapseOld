using Assets._Scripts.Dissonance;
using Synapse.Api;
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
        //Eigenschaften
        public CommandSender Sender { get; private set; }

        public ReferenceHub Player
        {
            get => Sender.SenderId == "SERVER CONSOLE" || Sender.SenderId == "GAME CONSOLE" ? ReferenceHub.GetHub(PlayerManager.localPlayer) : Synapse.Api.Player.GetPlayer(Sender.SenderId);
        }

        public bool Allow { get; set; }

        public string Command { get; private set; }

        //Konstruktor
        public RemoteCommandEvent(CommandSender sender,string command)
        {
            Sender = sender;
            Command = command;
        }
    }

    public class SpeakEvent : EventArgs
    {
        public ReferenceHub Player { get; }

        public DissonanceUserSetup DissonanceUserSetup { get; }

        public bool Scp939Talk { get; set; }

        public bool IntercomTalk { get; set; }

        public bool RadioTalk { get; set; }

        public bool ScpChat { get; set; }

        public bool SpectatorChat { get; set; }

        public SpeakEvent(ReferenceHub player, DissonanceUserSetup setup)
        {
            Player = player;
            DissonanceUserSetup = setup;
        }
    }

    public class Scp049RecallEvent : EventArgs
    {
        public ReferenceHub Player { get; private set; }

        public ReferenceHub Target { get; set; }

        public bool Allow { get; set; }

        public RoleType RespawnRole { get; set; }

        public float TargetHealth { get; set; }

        public Ragdoll Ragdoll { get; set; }

        public Scp049RecallEvent(ReferenceHub player)
        {
            Player = player;
        }
    }

    public class ConsoleCommandEvent
    {
        public ReferenceHub Player { get; internal set; }

        public string Command { get; internal set; }

        public string ReturnMessage { get; set; }

        public string Color { get; set; }
    }
}
