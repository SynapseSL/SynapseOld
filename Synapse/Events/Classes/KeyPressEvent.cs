using Synapse.Api;
using UnityEngine;

namespace Synapse.Events.Classes
{
    public class KeyPressEvent
    {
        public Player Player { get; internal set; }

        public KeyCode Key { get; internal set; }
    }
}
