using System.Diagnostics.CodeAnalysis;
using Synapse.Api;

namespace Synapse.Events.Classes
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")] 
    public class PlayerThrowGrenadeEvent
    {
        public Player Player { get; internal set; }

        public ItemType Grenade { get; internal set; }
        
        public bool Slow { get; set; }
        
        public double FuseTime { get; set; }

        public bool Allow { get; set; }
    }
}