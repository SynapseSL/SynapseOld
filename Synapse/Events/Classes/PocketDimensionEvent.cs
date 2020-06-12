using Synapse.Api;

namespace Synapse.Events.Classes
{
    public class PocketDimensionEvent
    {
        public Player Player { get; internal set; }
        public bool Allow { get; set; }
    }
}