using Synapse.Api;
using System.Diagnostics.CodeAnalysis;

namespace Synapse.Events.Classes
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class TeslaTriggerEvent
    {
        public Player Player { get; internal set; }

        public bool IsHurtRange { get; internal set; }

        public bool Triggerable { get; set; }
    }
}