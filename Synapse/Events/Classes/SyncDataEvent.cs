using Synapse.Api;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Synapse.Events.Classes
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class SyncDataEvent
    {
        public Player Player { get; internal set; }

        public int State { get; internal set; }

        public Vector2 Speed { get; set; }

        public bool Allow { get; set; } = true;
    }
}