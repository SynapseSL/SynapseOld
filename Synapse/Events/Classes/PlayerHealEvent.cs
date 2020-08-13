using System.Diagnostics.CodeAnalysis;
using Synapse.Api;

namespace Synapse.Events.Classes
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")] 
    public class PlayerHealEvent
    {
        public Player Player { get; internal set; }

        public float Amount { get; set; }
        
        public bool Allow { get; set; }
    }
}