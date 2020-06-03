using System.Diagnostics.CodeAnalysis;

namespace Synapse.Events.Classes
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class DoorInteractClass
    {
        public ReferenceHub Player { get; internal set; }

        public bool Allow { get; set; } = true;

        public Door Door { get; internal set; }
    }
}