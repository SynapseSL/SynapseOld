using Synapse.Api;
using System.Diagnostics.CodeAnalysis;

namespace Synapse.Events.Classes
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class PlayerBanEvent
    {
        public Player BannedPlayer { get; internal set; }

        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public bool Allowed { get; set; } = true;

        public int Duration { get; internal set; }

        public Player Issuer { get; internal set; }

        public string Reason { get; internal set; }
    }
}