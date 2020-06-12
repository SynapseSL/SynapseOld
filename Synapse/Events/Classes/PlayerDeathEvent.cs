using Synapse.Api;
using System.Diagnostics.CodeAnalysis;

namespace Synapse.Events.Classes
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class PlayerDeathEvent
    {
        public Player Player { get; internal set; }

        public Player Killer { get; internal set; }

        public PlayerStats.HitInfo Info { get; internal set; }
    }
}