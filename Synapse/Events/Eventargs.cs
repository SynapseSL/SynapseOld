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
}
