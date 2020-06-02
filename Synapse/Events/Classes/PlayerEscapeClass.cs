namespace Synapse.Events.Classes
{
    public class PlayerEscapeClass
    {
        public ReferenceHub Player { get; internal set; }

        public RoleType SpawnRole { get; set; }

        public bool Allow { get; set; }

        public RoleType CufferRole { get; internal set; }

        public bool IsCuffed { get; internal set; }
    }
}
