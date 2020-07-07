using Synapse.Api;

namespace Synapse.Events.Classes
{
    public class UseItemEvent
    {
        public Player Player { get; internal set; }

        public bool Allow { get; set; }
    }
}
