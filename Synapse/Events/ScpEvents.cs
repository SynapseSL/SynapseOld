using Synapse.Api;
using Synapse.Events.Classes;

namespace Synapse.Events
{
    public static partial class Events
    {
        /// <summary>A Event which is activated when Scp049 respawns a Player</summary>
        public delegate void OnScp049Recall(ref Scp049RecallEvent ev);
        public static event OnScp049Recall Scp049RecallEvent;

        internal static void InvokeScp049RecallEvent(Player player, ref Ragdoll ragdoll, ref Player target,
            ref bool allow, ref RoleType role, ref float lives)
        {
            if (Scp049RecallEvent == null) return;

            var ev = new Scp049RecallEvent
            {
                Allow = allow,
                Ragdoll = ragdoll,
                Target = target,
                RespawnRole = role,
                TargetHealth = lives,
                Player = player
            };

            Scp049RecallEvent.Invoke(ref ev);

            ragdoll = ev.Ragdoll;
            target = ev.Target;
            role = ev.RespawnRole;
            lives = ev.TargetHealth;
            allow = ev.Allow;
        }
        
        public delegate void OnPocketDimensionEnter(ref PocketDimensionEvent ev);
        public static event OnPocketDimensionEnter PocketDimensionEnterEvent;

        internal static void InvokePocketDimensionEnterEvent(Player player, ref bool allow)
        {
            if (PocketDimensionEnterEvent == null) return;

            var ev = new PocketDimensionEvent
            {
                Player = player,
                Allow = allow
            };
            
            PocketDimensionEnterEvent.Invoke(ref ev);

            allow = ev.Allow;
        }

        public delegate void OnScp106Containment(ref Scp106ContainmentEvent ev);
        public static event OnScp106Containment Scp106ContainmentEvent;

        internal static void InvokeScp106ContainmentEvent(Player player, ref bool allow)
        {
            if (Scp106ContainmentEvent == null) return;

            var ev = new Scp106ContainmentEvent
            {
                Player = player,
                Allow = allow
            };
            
            Scp106ContainmentEvent.Invoke(ref ev);

            allow = ev.Allow;
        }


        public delegate void OnScp079GainLvl(ref Scp079GainLvlEvent ev);
        public static event OnScp079GainLvl Scp079GainLvlEvent;
        internal static void InvokeScp079LvlEvent(Player player,ref int newlvl,ref bool allow)
        {
            if (Scp079GainLvlEvent == null) return;

            var ev = new Scp079GainLvlEvent
            {
                Allow = allow,
                NewLvl = newlvl,
                Player = player
            };

            Scp079GainLvlEvent.Invoke(ref ev);

            newlvl = ev.NewLvl;
            allow = ev.Allow;
        }
    }
}