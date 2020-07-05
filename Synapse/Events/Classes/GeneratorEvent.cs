using Synapse.Api;

namespace Synapse.Events.Classes
{
    public class GeneratorEvent
    {
        public Player Player { get; internal set; }

        public Generator079 Generator { get; internal set; }

        public bool Allow { get; set; }
    }
}
