using Synapse.Events.Classes;
using Synapse.Api;

namespace Synapse.Events
{
    public static partial class Events
    {
        public delegate void OnScp079GainLvl(Scp079GainLvlEvent ev);
        public static event OnScp079GainLvl Scp079GainLvlEvent;
        internal static void InvokeScp079LvlEvent(Player player, ref int newlvl, ref bool allow)
        {
            if (Scp079GainLvlEvent == null) return;

            var ev = new Scp079GainLvlEvent
            {
                Allow = allow,
                NewLvl = newlvl,
                Player = player
            };

            Scp079GainLvlEvent.Invoke(ev);

            newlvl = ev.NewLvl;
            allow = ev.Allow;
        }
    }
}
