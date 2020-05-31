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
            get => Sender.SenderId == "SERVER CONSOLE" || Sender.SenderId == "GAME CONSOLE" ? ReferenceHub.GetHub(PlayerManager.localPlayer) : ReferenceHub.GetHub(int.Parse(Sender.SenderId));
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
}
