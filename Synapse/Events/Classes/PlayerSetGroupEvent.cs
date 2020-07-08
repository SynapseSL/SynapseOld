using Synapse.Api;

namespace Synapse.Events.Classes
{
    public class PlayerSetGroupEvent
    {
        public Player Player { get; internal set; }

        public bool ByAdmin { get; internal set; }

        public UserGroup Group { get; set; }

        public bool RaAcces { get; set; }

        public bool Showtag { get; set; }

        public bool Allow { get; set; }
    }
}
