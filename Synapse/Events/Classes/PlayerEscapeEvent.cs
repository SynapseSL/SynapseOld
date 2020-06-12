using Synapse.Api;
using System.Diagnostics.CodeAnalysis;

namespace Synapse.Events.Classes
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class PlayerEscapeEvent
    {
        public Player Player { get; internal set; }

        public RoleType SpawnRole { get; set; }

        public bool Allow { get; set; }

        public RoleType CufferRole { get; internal set; }

        public bool IsCuffed { get; internal set; }
    }
}