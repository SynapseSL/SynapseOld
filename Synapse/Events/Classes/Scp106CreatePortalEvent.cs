using Synapse.Api;

namespace Synapse.Events.Classes
{
    public class Scp106CreatePortalEvent
    {
        public Player Player { get; internal set; }
        public bool Allow { get; set; }
    }
}