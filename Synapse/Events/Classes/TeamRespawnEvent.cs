using Synapse.Api;
using System.Collections.Generic;

namespace Synapse.Events.Classes
{
    public class TeamRespawnEvent
    {
        public Respawning.SpawnableTeamType Team { get; set; }

        public List<Player> RespawnList { get; set; }
    }
}
