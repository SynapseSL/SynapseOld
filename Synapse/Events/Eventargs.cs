using System;

namespace Synapse.Events
{
    public class PlayerJoinEvent : EventArgs
    {
        //Eigenschaften
        public ReferenceHub Player { get; private set; }

        public string Nick { get; set; }

        //Konstruktor
        public PlayerJoinEvent(ReferenceHub player)
        {
            Player = player;
        }
    }
}
