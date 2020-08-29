using Synapse.Api;
using Synapse.Events.Classes;

namespace Synapse.Events
{
    public static partial class Events
    {
        /// <summary>A Event which is activated when Scp049 respawns a Player</summary>
        public delegate void OnScp049Recall(Scp049RecallEvent ev);
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

            Scp049RecallEvent.Invoke(ev);

            ragdoll = ev.Ragdoll;
            target = ev.Target;
            role = ev.RespawnRole;
            lives = ev.TargetHealth;
            allow = ev.Allow;
        }
    }
}