using Synapse.Api;
using UnityEngine;

namespace Synapse.Events.Classes
{
    public class ShootEvent
    {
        public Player Player { get; internal set; }

        public Player Target { get; internal set; }

        public Vector3 TargetPosition { get; set; }

        public bool Allow { get; set; }
    }
}
