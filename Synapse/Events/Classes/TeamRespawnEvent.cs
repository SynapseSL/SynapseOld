using Synapse.Api;
using System.Collections.Generic;

namespace Synapse.Events.Classes
{
    public class TeamRespawnEvent
    {
        public bool IsChaos { get; set; }

        public List<Player> RespawnList { get; set; }

        public bool Allow { get; set; }

        public bool UseTickets { get; set; }
    }
}
