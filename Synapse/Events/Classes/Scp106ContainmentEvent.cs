namespace Synapse.Events.Classes
{
    public class Scp106ContainmentEvent
    {
        public ReferenceHub Player { get; internal set; }
        public bool Allow { get; set; }
    }
}