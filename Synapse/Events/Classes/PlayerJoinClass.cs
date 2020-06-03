using System;
using System.Diagnostics.CodeAnalysis;

namespace Synapse.Events.Classes
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class PlayerJoinClass : EventArgs
    {
        // Constructor
        public PlayerJoinClass(ReferenceHub player)
        {
            Player = player;
        }

        // Properties
        public ReferenceHub Player { get; }

        public string Nick { get; set; }
    }
}