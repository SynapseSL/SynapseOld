using Synapse.Api;
using System.Diagnostics.CodeAnalysis;

namespace Synapse.Events.Classes
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class PlayerCuffedEvent
    {
        public Player Cuffed { get; internal set; }

        public Player Target { get; internal set; }

        public bool Allow { get; set; }
    }
}