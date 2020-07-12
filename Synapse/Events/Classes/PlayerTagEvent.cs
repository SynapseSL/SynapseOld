using Synapse.Api;

namespace Synapse.Events.Classes
{
    public class PlayerTagEvent
    {
        public Player Player { get; internal set; }

        public bool ShowTag { get; internal set; }

        public bool Allow { get; set; }
    }
}
