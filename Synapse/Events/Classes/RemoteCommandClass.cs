using System;

namespace Synapse.Events.Classes
{
    public class RemoteCommandClass : EventArgs
    {
        public CommandSender Sender { get; internal set; }

        public ReferenceHub Player
        {
            get => Sender.SenderId == "SERVER CONSOLE" || Sender.SenderId == "GAME CONSOLE" ? ReferenceHub.GetHub(PlayerManager.localPlayer) : Api.Player.GetPlayer(Sender.SenderId);
        }

        public bool Allow { get; set; }

        public string Command { get; internal set; }
    }
}