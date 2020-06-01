namespace Synapse.Events.Classes
{
    public class PlayerDeathClass
    {
        public ReferenceHub Player { get; internal set; }

        public ReferenceHub Killer { get; internal set; }

        public PlayerStats.HitInfo Info { get; internal set; }
    }
}