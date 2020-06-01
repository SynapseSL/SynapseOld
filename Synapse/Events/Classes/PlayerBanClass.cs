namespace Synapse.Events.Classes
{
    public class PlayerBanClass
    {
        public ReferenceHub BannedPlayer { get; internal set; }

        public bool Allowed { get; set; } = true;
        
        public string UserId { get; internal set; }
        
        public int Duration { get; internal set; }
        
        public ReferenceHub Issuer { get; internal set; }
        
        public string Reason { get; internal set; }
    }
}