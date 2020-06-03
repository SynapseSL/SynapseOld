using System;

namespace Synapse.Events.Classes
{
    public class Scp049RecallClass : EventArgs
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public ReferenceHub Player { get; internal set; }

        public ReferenceHub Target { get; set; }

        public bool Allow { get; set; }

        public RoleType RespawnRole { get; set; }

        public float TargetHealth { get; set; }

        public Ragdoll Ragdoll { get; set; }
    }
}