using Synapse.Api;
using System;

namespace Synapse.Events.Classes
{
    public class Scp049RecallEvent : EventArgs
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public Player Player { get; internal set; }

        public Player Target { get; set; }

        public bool Allow { get; set; }

        public RoleType RespawnRole { get; set; }

        public float TargetHealth { get; set; }

        public Ragdoll Ragdoll { get; set; }
    }
}