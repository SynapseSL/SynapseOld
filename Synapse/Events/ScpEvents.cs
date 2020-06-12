using System.Diagnostics.CodeAnalysis;
using Synapse.Api;
using Synapse.Events.Classes;

namespace Synapse.Events
{
    public static partial class Events
    {
        /// <summary>A Event which is activated when Scp049 Respawnes a Player</summary>
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

            var ev = new PocketDimensionEvent()
            {
                Player = player,
                Allow = allow
            };
            
            PocketDimensionEnterEvent.Invoke(ref ev);

            allow = ev.Allow;
        }

        public delegate void OnScp106Containment(ref Scp106ContainmentEvent ev);
        public static event OnScp106Containment Scp106ContaimentEvent;

        internal static void InvokeScp106ContaimentEvent(Player player, ref bool allow)
        {
            if (Scp106ContaimentEvent == null) return;

            var ev = new Scp106ContainmentEvent()
            {
                Player = player,
                Allow = allow
            };
            
            Scp106ContaimentEvent.Invoke(ref ev);

            allow = ev.Allow;
        }
    }
}