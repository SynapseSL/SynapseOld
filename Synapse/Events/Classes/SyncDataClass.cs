using UnityEngine;

namespace Synapse.Events.Classes
{
    public class SyncDataClass
    {
        public ReferenceHub Player { get; internal set; }
        
        public int State { get; internal set; }
        
        public Vector2 Speed { get; set; }

        public bool Allow { get; set; } = true;
    }
}