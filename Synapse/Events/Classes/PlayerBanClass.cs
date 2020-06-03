using System.Diagnostics.CodeAnalysis;

namespace Synapse.Events.Classes
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class PlayerBanClass
    {
        public ReferenceHub BannedPlayer { get; internal set; }

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public bool Allowed { get; set; } = true;

        public string UserId { get; internal set; }

        public int Duration { get; internal set; }

        public ReferenceHub Issuer { get; internal set; }

        public string Reason { get; internal set; }
    }
}