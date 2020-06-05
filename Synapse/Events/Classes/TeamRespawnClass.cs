using System.Collections.Generic;

namespace Synapse.Events.Classes
{
    public class TeamRespawnClass
    {
        public bool IsChaos { get; set; }

        public List<ReferenceHub> RespawnList { get; set; }

        public bool Allow { get; set; }

        public bool UseTickets { get; set; }
    }
}
