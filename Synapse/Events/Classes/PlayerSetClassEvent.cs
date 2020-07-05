using System.Collections.Generic;
using Synapse.Api;

namespace Synapse.Events.Classes
{
    public class PlayerSetClassEvent
    {
        public Player Player { get; internal set; }
        public RoleType Role { get; set; }
        public List<ItemType> Items { get; set; }
    }
}