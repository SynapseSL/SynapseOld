namespace Synapse.Events.Classes
{
    public class PocketDimensionEvent
    {
        public ReferenceHub Player { get; internal set; }
        public bool Allow { get; set; }
    }
}