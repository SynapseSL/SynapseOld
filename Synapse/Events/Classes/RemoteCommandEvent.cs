﻿using System;

namespace Synapse.Events.Classes
{
    public class RemoteCommandEvent : EventArgs
    {
        public CommandSender Sender { get; internal set; }

        public ReferenceHub Player => Sender.SenderId == "SERVER CONSOLE" || Sender.SenderId == "GAME CONSOLE"
            ? ReferenceHub.GetHub(PlayerManager.localPlayer)
            : Api.Player.GetPlayer(Sender.SenderId);

        public bool Allow { get; set; }

        public string Command { get; internal set; }
    }
}