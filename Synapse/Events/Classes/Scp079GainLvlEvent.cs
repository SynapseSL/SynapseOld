using Synapse.Api;

namespace Synapse.Events.Classes
{
    public class Scp079GainLvlEvent
    {
        public Player Player { get; internal set; }

        public bool Allow { get; set; }

        public int NewLvl { get; set; }
    }
}
