using System.Diagnostics.CodeAnalysis;
using Synapse.Events.Classes;

namespace Synapse.Events
{
    [SuppressMessage("ReSharper", "EventNeverSubscribedTo.Global")]
    public static partial class Events
    {
        /// <summary>A Event which is activated when Scp049 Respawnes a Player</summary>
        public delegate void OnScp049Recall(ref Scp049RecallEvent ev);
        public static event OnScp049Recall Scp049RecallEvent;

        internal static void InvokeScp049RecallEvent(ReferenceHub player, ref Ragdoll ragdoll, ref ReferenceHub target,
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
    }
}