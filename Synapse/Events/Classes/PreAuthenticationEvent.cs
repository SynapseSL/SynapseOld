using LiteNetLib;

namespace Synapse.Events.Classes
{
    public class PreAuthenticationEvent
    {
        public string UserId { get; internal set; }
        public ConnectionRequest Request { get; internal set; }
        public bool Allow { get; set; }
    }
}