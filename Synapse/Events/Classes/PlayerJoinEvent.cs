using Synapse.Api;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Synapse.Events.Classes
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class PlayerJoinEvent : EventArgs
    {
        public Player Player { get; internal set; }

        public string Nick { get; set; }
    }
}