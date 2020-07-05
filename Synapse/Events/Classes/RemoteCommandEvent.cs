using Synapse.Api;
using System;

namespace Synapse.Events.Classes
{
    public class RemoteCommandEvent : EventArgs
    {
        public CommandSender Sender { get; internal set; }

        public Player Player => Sender.SenderId == "SERVER CONSOLE" || Sender.SenderId == "GAME CONSOLE"
            ? Player.Server
            : PlayerExtensions.GetPlayer(Sender.SenderId);

        public bool Allow { get; set; }

        public string Command { get; internal set; }
    }
}