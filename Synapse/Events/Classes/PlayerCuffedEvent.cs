using System.Diagnostics.CodeAnalysis;

namespace Synapse.Events.Classes
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class PlayerCuffedEvent
    {
        public ReferenceHub Cuffed { get; internal set; }

        public ReferenceHub Target { get; internal set; }

        public bool Allow { get; set; }
    }
}