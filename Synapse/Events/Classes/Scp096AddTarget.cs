using Synapse.Api;

namespace Synapse.Events.Classes
{
    public class Scp096AddTarget
    {
        public Player Player { get; internal set; }

        public Player ShyGuy { get; internal set; }

        public PlayableScps.Scp096PlayerState RageState { get; internal set; }

        public bool Allow { get; set; }
    }
}
