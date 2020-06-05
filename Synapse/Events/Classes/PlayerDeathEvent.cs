using System.Diagnostics.CodeAnalysis;

namespace Synapse.Events.Classes
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class PlayerDeathEvent
    {
        public ReferenceHub Player { get; internal set; }

        public ReferenceHub Killer { get; internal set; }

        public PlayerStats.HitInfo Info { get; internal set; }
    }
}