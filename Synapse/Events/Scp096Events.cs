using Synapse.Events.Classes;
using Synapse.Api;

namespace Synapse.Events
{
    public static partial class Events
    {
        public delegate void OnScp096AddTarget(Scp096AddTarget ev);
        public static event OnScp096AddTarget Scp096AddTarget;
        internal static void InvokeScp096AddTarget(Player player, Player shyguy, PlayableScps.Scp096PlayerState state, ref bool allow)
        {
            if (Scp096AddTarget == null) return;

            var ev = new Scp096AddTarget()
            {
                Player = player,
                ShyGuy = shyguy,
                RageState = state,
                Allow = allow
            };

            Scp096AddTarget.Invoke(ev);

            allow = ev.Allow;
        }
    }
}
