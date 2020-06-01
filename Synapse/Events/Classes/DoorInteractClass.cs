namespace Synapse.Events.Classes
{
    public class DoorInteractClass
    {
        public ReferenceHub Player { get; internal set; }

        public bool Allow { get; set; } = true;
        
        public Door Door { get; internal set; }
    }
}