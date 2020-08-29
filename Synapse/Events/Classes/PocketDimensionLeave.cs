using Synapse.Api;

namespace Synapse.Events.Classes
{
    public class PocketDimensionLeave
    {
        public Player Player { get; internal set; }

        public PocketDimensionTeleport.PDTeleportType TeleportType { get; set; }

        public bool Allow { get; set; }
    }
}
